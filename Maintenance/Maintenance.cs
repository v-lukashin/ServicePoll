using System.Text.RegularExpressions;
using ServicePoll.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ServicePoll.Maintenance
{
    public static class Maintenance
    {
        public static IEnumerable<string> Processing(TempUrlRepository tempUrlRepository, string pollId, int limitItems)
        {
            var urlList = new HashSet<string>();

            Console.Write("Подбираю URL`ы...");

            var sw = Stopwatch.StartNew();

            var all = tempUrlRepository.GetShuffleUrls((int)(limitItems * 1.5)); // берем больше, т.к. может быть много брака или повторов 
            
            foreach (var url in all)
            {
                if (urlList.Count < limitItems)
                {
                    var modifUrl = new Regex(@"(?<=(https?://)?[-\w.]+)[/#?].*").Replace(url, string.Empty);//url.Replace(uri.PathAndQuery, string.Empty);

                    if (urlList.Contains(modifUrl)) continue;
                    urlList.Add(modifUrl);
                }
                else break;
            }
            sw.Stop();
            Console.WriteLine("завершил. Время работы : {0}ms", sw.ElapsedMilliseconds);
            return urlList;
        }
    }
}