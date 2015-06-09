using System.Web.Http;

using Newtonsoft.Json.Serialization;

namespace APIHypermediaPrototype
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            //Don't need this anymore.
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional });

            //Don't need this anymore.
            config.Routes.MapHttpRoute(
                name: "Default",
               routeTemplate: "{controller}",
                defaults: new { controller = "bookmark" });

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
