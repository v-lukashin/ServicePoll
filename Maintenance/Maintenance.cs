using ServicePoll.Models;
using ServicePoll.Repository;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ServicePoll.Maintenance
{
    public static class Maintenance
    {
        public static IEnumerable<string> Processing(TempUrlRepository tempUrlRepository, string pollId, int limitItems)
        {
            HashSet<string> urlList = new HashSet<string>();
            int countUrlView = 0;

            var all = tempUrlRepository.GetShuffleUrls((int)(limitItems * 1.5)); // берем больше, т.к. много брака или повторов 

            Console.Write("Подбираю URL`ы");

            Stopwatch sw = Stopwatch.StartNew();
            foreach (var url in all)
            {
                if (urlList.Count < limitItems)
                {
                    countUrlView++;

                    Uri uri;
                    try
                    {
                        uri = new Uri(url);
                    }
                    catch (UriFormatException e) { Console.WriteLine("Невалидный Url"); continue; }

                    var modifUrl = url.Replace(uri.PathAndQuery, string.Empty);

                    if (!urlList.Contains(modifUrl))
                    {
                        urlList.Add(modifUrl);
                        Console.Write('.');
                    }
                }
                else { break; }
            }
            sw.Stop();
            Console.WriteLine("завершил. Время работы : {0}ms", sw.ElapsedMilliseconds);
            return urlList;
        }
    }
}