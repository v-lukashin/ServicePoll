﻿using ServicePoll.Repository;
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

            var all = tempUrlRepository.GetShuffleUrls((int)(limitItems * 1.5)); // берем больше, т.к. много брака или повторов 

            Console.Write("Подбираю URL`ы");

            var sw = Stopwatch.StartNew();
            foreach (var url in all)
            {
                if (urlList.Count < limitItems)
                {
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