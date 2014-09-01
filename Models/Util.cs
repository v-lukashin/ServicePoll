using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace ServicePoll.Models
{
    public static class Util
    {
        public static string GetCookie(this HttpContext context, string name)
        {
            var res = context.Request.Cookies.Get(name);
            return res == null ? null : res.Value;
        }
        public static void SetCookie(this HttpContext context, string name, string value)
        {
            context.Response.SetCookie(new HttpCookie(name, value));
        }

        public static string GenerateIdBasedPollIdAndUrl(string pollId, string url)
        {
            var resStr = pollId + url;
            var bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(resStr));
            resStr = BitConverter.ToString(bytes).Replace("-", string.Empty);
            return resStr;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> en)
        {
            IList<T> list = en as T[] ?? en.ToArray<T>();
            int n = list.Count();
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        public static class ThreadSafeRandom
        {
            [ThreadStatic]
            private static Random _local;

            public static Random ThisThreadsRandom
            {
                get { return _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
            }
        }

        public static void GetFieldNameAndExpression(FieldName fName, out string name, out Func<TempUrl, string> expr)
        {
            switch (fName)
            {
                case FieldName.PageUrl:
                    name = "PageUrl";
                    expr = x => x.PageUrl;
                    return;
                case FieldName.Uri:
                    name = "Uri";
                    expr = x => x.Uri;
                    return;
                case FieldName.Url:
                    name = "Url";
                    expr = x => x.Url;
                    return;
            }
            throw new Exception(string.Format("Поле с именем {0} не поддерживается", fName));
        }
    }
}