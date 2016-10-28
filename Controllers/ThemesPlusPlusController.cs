using System.Web.Mvc;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Development.ThemesPlusPlus.Controllers
{
    [AdminAuthorize]
    public class ThemesPlusPlusController : BasePluginController
    {
        [ChildActionOnly]
        public ActionResult Configure()
        {
            return View("~/Plugins/Devemopment.ThemesPlusPlus/Views/ThemesPlusPlus/Configure.cshtml");
        }
    }
}
