using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class ThemesPlusPlusViewEngine : ThemeableRazorViewEngine
    {
        protected override string GetPathFromGeneralName(
            ControllerContext controllerContext, 
            List<ViewLocation> locations, 
            string name, 
            string controllerName, 
            string areaName, 
            string theme, 
            string cacheKey, 
            ref string[] searchedLocations)
        {
            var parentThemes = LoadParentThemes(theme);

            string result = string.Empty;
            var checkedLocations = new List<string>();

            for (int i = 0; i < locations.Count; i++)
            {
                var location = locations[i];
                string virtualPath = location.Format(name, controllerName, areaName, theme);
                var virtualPathDisplayInfo = DisplayModeProvider.GetDisplayInfoForVirtualPath(
                    virtualPath, 
                    controllerContext.HttpContext, 
                    path => FileExists(controllerContext, path), 
                    controllerContext.DisplayMode);

                if (virtualPathDisplayInfo == null && virtualPath.IndexOf("/Themes/", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    // We're checking themes location. Try parent themes as well.

                    foreach(var parentTheme in parentThemes)
                    {
                        // 1. Track the last checked location
                        checkedLocations.Add(virtualPath);

                        // 2. Try parent theme
                        virtualPath = location.Format(name, controllerName, areaName, parentTheme);
                        virtualPathDisplayInfo = DisplayModeProvider.GetDisplayInfoForVirtualPath(
                            virtualPath,
                            controllerContext.HttpContext,
                            path => FileExists(controllerContext, path),
                            controllerContext.DisplayMode);

                        if (virtualPathDisplayInfo != null)
                        {
                            // 3. We found target view in the parent theme
                            break;
                        }
                    }
                }

                if (virtualPathDisplayInfo != null)
                {
                    string resolvedVirtualPath = virtualPathDisplayInfo.FilePath;

                    checkedLocations.Clear();
                    result = resolvedVirtualPath;
                    ViewLocationCache.InsertViewLocation(
                        controllerContext.HttpContext, 
                        AppendDisplayModeToCacheKey(cacheKey, virtualPathDisplayInfo.DisplayMode.DisplayModeId), result);

                    if (controllerContext.DisplayMode == null)
                    {
                        controllerContext.DisplayMode = virtualPathDisplayInfo.DisplayMode;
                    }

                    var allDisplayModes = DisplayModeProvider.Modes;
                    foreach (IDisplayMode displayMode in allDisplayModes)
                    {
                        if (displayMode.DisplayModeId != virtualPathDisplayInfo.DisplayMode.DisplayModeId)
                        {
                            var displayInfoToCache = displayMode.GetDisplayInfo(controllerContext.HttpContext, virtualPath, virtualPathExists: path => FileExists(controllerContext, path));

                            string cacheValue = string.Empty;
                            if (displayInfoToCache != null && displayInfoToCache.FilePath != null)
                            {
                                cacheValue = displayInfoToCache.FilePath;
                            }

                            ViewLocationCache.InsertViewLocation(
                                controllerContext.HttpContext, 
                                AppendDisplayModeToCacheKey(cacheKey, displayMode.DisplayModeId), cacheValue);
                        }
                    }

                    break;
                }

                checkedLocations.Add(virtualPath);
            }

            searchedLocations = checkedLocations.ToArray();
            return result;
        }

        private IList<string> LoadParentThemes(string currentTheme)
        {
            var provider = EngineContext.Current.Resolve<IThemeProvider>();
            var parentThemes = new List<string>();
            var theme = provider.GetThemeConfiguration(currentTheme) as ThemePlusPlusConfiguration;
            while (theme.ParentTheme != null)
            {
                theme = provider.GetThemeConfiguration(theme.ParentTheme) as ThemePlusPlusConfiguration;
                if (theme == null)
                {
                    // Parent theme is not found
                    break;
                }

                parentThemes.Add(theme.ThemeName);
            }

            return parentThemes;
        }
    }
}
