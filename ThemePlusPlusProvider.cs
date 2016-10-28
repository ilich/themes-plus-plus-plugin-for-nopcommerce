using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class ThemePlusPlusProvider : IThemeProvider
    {
        private readonly IList<ThemeConfiguration> _themeConfigurations = new List<ThemeConfiguration>();

        private readonly bool _isPluginEnabled;

        public ThemePlusPlusProvider(IPluginFinder pluginFinder)
        {
            var plugin = pluginFinder.GetPluginDescriptorBySystemName("Devemopment.ThemesPlusPlus");
            _isPluginEnabled = plugin?.Installed == true;
            LoadThemes();
        }

        public ThemeConfiguration GetThemeConfiguration(string themeName)
        {
            return _themeConfigurations.FirstOrDefault(c => string.Compare(c.ThemeName, themeName, true) == 0);
        }

        public IList<ThemeConfiguration> GetThemeConfigurations()
        {
            return _themeConfigurations;
        }

        public bool ThemeConfigurationExists(string themeName)
        {
            return _themeConfigurations.Any(c => string.Compare(c.ThemeName, themeName, true) == 0);
        }

        private void LoadThemes()
        {
            var basePath = CommonHelper.MapPath("~/Themes/");

            foreach (string themeName in Directory.GetDirectories(basePath))
            {
                var configuration = CreateThemeConfiguration(themeName);
                if (configuration != null)
                {
                    _themeConfigurations.Add(configuration);
                }
            }
        }

        private ThemeConfiguration CreateThemeConfiguration(string themePath)
        {
            var themeDirectory = new DirectoryInfo(themePath);
            var themeConfigFile = new FileInfo(Path.Combine(themeDirectory.FullName, "theme.config"));

            if (!themeConfigFile.Exists)
            {
                return null;
            }

            var doc = new XmlDocument();
            doc.Load(themeConfigFile.FullName);
            var themeConfig = _isPluginEnabled
                ? new ThemePlusPlusConfiguration(themeDirectory.Name, themeDirectory.FullName, doc)
                : new ThemeConfiguration(themeDirectory.Name, themeDirectory.FullName, doc);
            return themeConfig;
        }
    }
}
