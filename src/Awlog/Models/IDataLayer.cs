using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awlog.Models
{
    public interface IDataLayer
    {
        IEnumerable<BPost> GetUsersPosts(int userId);
        Task SavePost(int userId, BPost post);
        BUser GetUserInfo(int userId);
        Guid GetAuthToken(int userId);
        int GetIdByLogPass(string login, string passwordHash);
        int Registration(RegForm newUser);
        int GetIdByToken(Guid token);
        bool IsLoginValid(string login);
    }
}
