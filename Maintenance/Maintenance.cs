using ServicePoll.Logic;
using ServicePoll.Models;
using ServicePoll.Repository;
using ServicePoll.Repository.Mongo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Maintenance
{
    public class Maintenance
    {
        private Repository<Poll> _pollRepository;
        private ItemRepository _itemRepository;
        private SiteIndexRepository _siteIndexRepository;

        public Maintenance(Repository<Poll> pollRep, ItemRepository itemRep, SiteIndexRepository siteIndexRep)
        {
            _pollRepository = pollRep;
            _itemRepository = itemRep;
            _siteIndexRepository = siteIndexRep;
        }
        public void Processing(string pollId, int limitItems)
        {
            HashSet<string> _urlList = new HashSet<string>();
            int countUrls = 0;
            int countUrlView = 0;

            var all = _siteIndexRepository.GetShuffleUrls((int)(limitItems * 1.5)); // берем больше, т.к. много брака или повторов 

            Console.Write("Подбираю URL`ы");

            Stopwatch sw = Stopwatch.StartNew();
            foreach (var url in all)
            {
                if (countUrls < limitItems)
                {
                    countUrlView++;

                    Uri uri;
                    try
                    {
                        uri = new Uri(url);
                    }
                    catch (UriFormatException e) { Console.WriteLine("Невалидный Url"); continue; }

                    var modifUrl = url.Replace(uri.PathAndQuery, string.Empty);

                    if (!_urlList.Contains(modifUrl))
                    {
                        _urlList.Add(modifUrl);
                        countUrls++;
                        Console.Write('.');
                    }
                }
                else { break; }
            }
            sw.Stop();
            Console.WriteLine("завершил. Время работы : {0}ms", sw.ElapsedMilliseconds);
            var poll = _pollRepository.Get(pollId);
            poll.IsActive = true;
            _pollRepository.Update(poll.Id, poll);

            foreach (var url in _urlList)
            {
                _itemRepository.Create(new Item(url, pollId));
            }
        }
    }
}