using System.Linq;
using ServicePoll.Config;
using ServicePoll.Models;
using ServicePoll.Repository;
using System;
using System.Collections.Generic;

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
                        var issueNames = new List<string>();
                        var answersNames = new List<string[]>();
                        while (true)
                        {
                            Console.WriteLine("Введите вопрос или end:");

                            var issueName = Console.ReadLine();
                            if(issueName.ToLower() == "end") break;

                            var answers = new List<string>();
                            while (true)
                            {
                                Console.WriteLine("Введите ответ или end:");
                                var ans = Console.ReadLine();
                                if (ans.ToLower() == "end") break;
                                answers.Add(ans);
                            }
                            issueNames.Add(issueName);
                            answersNames.Add(answers.ToArray());
                        }
                        FillWithMaintenance(pollName, limit, urlCount, issueNames, answersNames, debug);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: {0}\nПопробуйте снова. Проверьте подключение к базе данных и вводимые данные.", e);
                }
            }
        }
        private static void FillWithMaintenance(string pollName, int limit, int urlCount, IList<string> issueName, IList<string[]> answerNames, bool isDebug)
        {
            if(issueName.Count != answerNames.Count)throw new Exception("Количество вопросов отличается от числа групп ответов");
            var connStr = ServicePollConfig.PollConnectionString;//"mongodb://localhost:27017/polls";
            var pollRep = new RepositoryGeneric<Poll>(new MongoDb<Poll>(connStr));
            var itemRep = new ItemRepository(new MongoDb<Item>(connStr));
            var tempRep = new TempUrlRepository(new MongoDb<TempUrl>(ServicePollConfig.TempConnectionString));
            var issueRep = new IssueRepository(new MongoDb<Issue>(connStr));
            var answerRep = new AnswerRepository(new MongoDb<Answer>(connStr));

            var poll = new Poll(pollName, limit, true);

            var urlList = Maintenance.Processing(tempRep, poll.Id, urlCount);

            var resIssues = new List<Issue>();
            var resAnswers = new List<Answer>();

            for (var i = 0; i < issueName.Count; i++)
            {
                var issue = new Issue(issueName[i], poll.Id, IssueType.Single);
                var answers = answerNames[i].Select(answ => new Answer(answ, issue.Id)).ToList();    
                resIssues.Add(issue);
                resAnswers.AddRange(answers);
            }
            
            if (!isDebug)
            {
                pollRep.Create(poll);
                foreach (var url in urlList)
                {
                    itemRep.Create(new Item(url, poll.Id));
                }

                foreach (var issue in resIssues)
                {
                    issueRep.Create(issue);
                }

                foreach (var ans in resAnswers)
                {
                    answerRep.Create(ans);
                }
            }
            else Console.WriteLine("Nothing is saved (Debug mode)");
        }
    }
}
