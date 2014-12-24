using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Owin;
using PositiveTechnologies.Fibonacci.CalculationService.StructureMapDependencyResolution;
using StructureMap;

namespace PositiveTechnologies.Fibonacci.CalculationService
{
    /// <summary>
    /// Represents Web Application startup configuration.
    /// </summary>
    internal sealed class Startup
    {
        private readonly IContainer _container;
        private readonly IExceptionLogger _exceptionLogger;

        public Startup(IContainer container, IExceptionLogger exceptionLogger)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (exceptionLogger == null) throw new ArgumentNullException("exceptionLogger");

            _container = container;
            _exceptionLogger = exceptionLogger;
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Add(typeof(IExceptionLogger), _exceptionLogger);
            config.DependencyResolver = new StructureMapDependencyResolver(_container);
            
            appBuilder.UseWebApi(config);
        }
    }
}
