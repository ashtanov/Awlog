using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Awlog
{
    public static class Extensions
    {
        private static readonly MD5 Md5Gen = MD5.Create();
        public static string GetMd5Hash(this string @this)
        {
            var res = Md5Gen.ComputeHash(Encoding.UTF8.GetBytes(@this));
            var sb = new StringBuilder();
            foreach (var x in res)
                sb.Append(x.ToString("X2"));
            return sb.ToString();
        }
    }
}
