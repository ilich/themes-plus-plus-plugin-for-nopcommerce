using System;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Common;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class ThemesPlusPlusPlugn : BasePlugin, IMiscPlugin
    {
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            throw new NotSupportedException();
        }
    }
}
