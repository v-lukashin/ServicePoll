using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicePoll.Logic;
using ServicePoll.Repository.Mongo;
using ServicePoll.Models;

namespace Maintenance
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите название опроса или exit для выхода:");
                    var pollName = Console.ReadLine();
                    if (pollName.ToLower() == "exit")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Введите лимит количества респондентов:");
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
                        FillWithMaintenance(pollName, limit, urlCount, issueName, answers);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка. Попробуйте снова. Проверьте подключение к базе данных и вводимые данные.");
                }
            }
        }
        private static void FillWithMaintenance(string pollName, int limit, int urlCount, string issueName, IEnumerable<string> answerNames)
        {
            string connStr = "mongodb://localhost:27017/polls";
            Repository<Poll> pollRep = new Repository<Poll>(new MongoDb<Poll>(connStr));
            ItemRepository ItemRep = new ItemRepository(new MongoDb<Item>(connStr));
            SiteIndexRepository siRep = new SiteIndexRepository();
            IssueRepository IssueRep = new IssueRepository(new MongoDb<Issue>(connStr));
            AnswerRepository AnswerRep = new AnswerRepository(new MongoDb<Answer>(connStr));

            var maint = new Maintenance(pollRep, ItemRep, siRep);
            Poll poll = new Poll(pollName, limit);
            pollRep.Create(poll);

            maint.Processing(poll.Id, urlCount);

            Issue issue = new Issue(issueName, poll.Id, IssueType.Single);
            List<Answer> answers = new List<Answer>();
            foreach (var answ in answerNames)
            {
                answers.Add(new Answer(answ, issue.Id));
            }
            IssueRep.Create(issue);
            foreach (var ans in answers)
            {
                AnswerRep.Create(ans);
            }
        }
    }
}
