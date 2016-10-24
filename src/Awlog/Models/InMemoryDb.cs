using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awlog.Models
{

    public class InMemoryDb : IDataLayer
    {
        private class Token
        {
            public Guid token { get; set; }
            public bool IsClosed { get; set; }
            public DateTime Expiries { get; set; }
        }

        private readonly ConcurrentDictionary<int, List<BPost>> _posts;
        private readonly ConcurrentDictionary<int, BUser> _users;
        private readonly ConcurrentDictionary<int, List<Token>> _auth;
        private readonly ConcurrentDictionary<Guid, int> _authInverse;

        public InMemoryDb()
        {
            _posts = new ConcurrentDictionary<int, List<BPost>>();
            _users = new ConcurrentDictionary<int, BUser>();
            _auth = new ConcurrentDictionary<int, List<Token>>();
            _authInverse = new ConcurrentDictionary<Guid, int>();
        }
        public IEnumerable<BPost> GetUsersPosts(int userId)
        {
            return _posts[userId];
        }

        public async Task SavePost(int userId, BPost post)
        {
            await Task.Run(() =>
            {
                List<BPost> tmp;
                if (_posts.TryGetValue(userId, out tmp))
                    tmp.Add(post);
                else
                    _posts.AddOrUpdate(userId, new List<BPost> { post }, ((i, list) =>
                    {
                        list.Add(post);
                        return list;
                    }));
            });
        }

        public BUser GetUserInfo(int userId)
        {
            return _users[userId];
        }

        public Guid GetAuthToken(int userId)
        {

            var nt = new Token { Expiries = DateTime.Now.AddDays(10), token = Guid.NewGuid(), IsClosed = false };
            List<Token> t;
            if (_auth.TryGetValue(userId, out t))
            {
                lock (t)
                {
                    foreach (var token in t)
                        token.IsClosed = true;
                    _authInverse.TryAdd(nt.token, userId);
                    t.Add(nt);
                }
            }
            else
            {
                _auth.TryAdd(userId, new List<Token> { nt });
                _authInverse.TryAdd(nt.token, userId);
            }
            return nt.token;
        }

        public int GetIdByLogPass(string login, string passwordHash)
        {
            var res = _users.Where(x => x.Value.Login == login && x.Value.PasswordHash == passwordHash);
            if (res.Any())
                return res.First().Key;
            return -1;
        }

        public int Registration(RegForm newUser)
        {
            if (_users.Values.Any(x => x.Login == newUser.Login))
                throw new Exception("Login already exist!");
            var nu = new BUser(_users.Count, newUser.Login, newUser.Name, newUser.Surname, newUser.Email,
                newUser.Password.GetMd5Hash());
            if (!_users.TryAdd(nu.Id, nu))
                throw new Exception("Something went wrong...");
            return nu.Id;

        }

        public int GetIdByToken(Guid token)
        {
            int id;
            if (!_authInverse.TryGetValue(token, out id)) return -1;
            var t = _auth[id];
            lock (t)
            {
                if (t.Any(x => x.token == token && !x.IsClosed))
                    return id;
            }
            return -1;
        }

        public bool IsLoginValid(string login)
        {
            return !string.IsNullOrWhiteSpace(login) && _users.Values.Any(x => x.Login == login);
        }
    }
}
