using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Common;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class ThemesPlusPlusPlugin : BasePlugin, IMiscPlugin
    {
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "ThemesPlusPlus";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Development.ThemesPlusPlus.Controllers" }, { "area", null } };
        }
    }
}
