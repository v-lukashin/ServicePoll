using ServicePoll;
using ServicePoll.Config;
using ServicePoll.Models;
using ServicePoll.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
namespace ServicePoll.Maintenance
{
    class Program
    {
        static void Main(string[] args)
        {
            var debug = false;
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите название опроса или exit для выхода:");
                    var pollName = Console.ReadLine();
                    
                    if(pollName.ToLower() == "debug"){
                        Console.WriteLine("Debug mode: On");
                        debug = true;
                    }
                    else if (pollName.ToLower() == "exit")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Введите число респондентов, необходимых для одного URL:");
                        var limit = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите количество сайтов для опроса:");
                        var urlCount = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите вопрос:");
                        var issueName = Console.ReadLine();
                        var answers = new List<string>();
                        while (true)
                        {
                            Console.WriteLine("Введите ответ или end, если Вы закончили вводить ответы:");
                            var ans = Console.ReadLine();
                            if (ans.ToLower() == "end") break;
                            answers.Add(ans);
                        }
                        FillWithMaintenance(pollName, limit, urlCount, issueName, answers, debug);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: {0}\nПопробуйте снова. Проверьте подключение к базе данных и вводимые данные.", e);
                }
            }
        }
        private static void FillWithMaintenance(string pollName, int limit, int urlCount, string issueName, IEnumerable<string> answerNames, bool isDebug)
        {
            string connStr = ServicePollConfig.PollConnectionString;//"mongodb://localhost:27017/polls";
            RepositoryGeneric<Poll> pollRep = new RepositoryGeneric<Poll>(new MongoDb<Poll>(connStr));
            ItemRepository itemRep = new ItemRepository(new MongoDb<Item>(connStr));
            TempUrlRepository tempRep = new TempUrlRepository(new MongoDb<TempUrl>(ServicePollConfig.TempConnectionString));
            IssueRepository issueRep = new IssueRepository(new MongoDb<Issue>(connStr));
            AnswerRepository answerRep = new AnswerRepository(new MongoDb<Answer>(connStr));

            Poll poll = new Poll(pollName, limit, true);

            var urlList = Maintenance.Processing(tempRep, poll.Id, urlCount);

            Issue issue = new Issue(issueName, poll.Id, IssueType.Single);
            List<Answer> answers = new List<Answer>();
            foreach (var answ in answerNames)
            {
                answers.Add(new Answer(answ, issue.Id));
            }

            if (!isDebug)
            {
                pollRep.Create(poll);
                foreach (var url in urlList)
                {
                    itemRep.Create(new Item(url, poll.Id));
                }

                issueRep.Create(issue);
                foreach (var ans in answers)
                {
                    answerRep.Create(ans);
                }
            }
            else Console.WriteLine("Nothing is saved (Debug mode)");
        }
    }
}
