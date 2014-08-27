using Ninject;
using ServicePoll.Infrastructure;
using System.Web.Http;

namespace ServicePoll
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "PollApi",
                routeTemplate: "api/{pollId}/skip",
                defaults: new { controller = "general" }
                );
            config.Routes.MapHttpRoute(
                name: "PollApi1",
                routeTemplate: "api/{pollId}/next",
                defaults: new { controller = "general" }
                );
            config.Routes.MapHttpRoute(
                name: "PollApi2",
                routeTemplate: "api/{pollId}/ok",
                defaults: new { controller = "general" }
                );
            config.Routes.MapHttpRoute(
                name: "PollApi3",
                routeTemplate: "api/{pId}/issue",
                defaults: new { controller = "general" }
                );

            config.Routes.MapHttpRoute(
                name: "PollApi4",
                routeTemplate: "api/poll",
                defaults: new { controller = "pollapi" }
                );
            config.Routes.MapHttpRoute(
                name: "DeafaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            // Раскомментируйте следующую строку кода, чтобы включить поддержку запросов для действий с типом возвращаемого значения IQueryable или IQueryable<T>.
            // Чтобы избежать обработки неожиданных или вредоносных запросов, используйте параметры проверки в QueryableAttribute, чтобы проверять входящие запросы.
            // Дополнительные сведения см. по адресу http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // Чтобы отключить трассировку в приложении, закомментируйте или удалите следующую строку кода
            // Дополнительные сведения см. по адресу: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
            config.DependencyResolver = new DependencyResolver(new StandardKernel());
        }
    }
}
