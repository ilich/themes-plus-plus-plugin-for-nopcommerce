using System.Web.Mvc;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority => 1;

        public void RegisterRoutes(RouteCollection routes)
        {
            var pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
            var plugin = pluginFinder.GetPluginDescriptorBySystemName("Devemopment.ThemesPlusPlus");
            bool isPluginEnabled = plugin?.Installed == true;

            if (!isPluginEnabled)
            {
                return;
            }

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemesPlusPlusViewEngine());
        }
    }
}
