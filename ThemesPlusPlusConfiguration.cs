using System.Xml;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class ThemesPlusPlusConfiguration : ThemeConfiguration
    {
        public ThemesPlusPlusConfiguration(string themeName, string path, XmlDocument doc)
            : base(themeName, path, doc)
        {
            var attribute = ConfigurationNode.Attributes["parentTheme"];
            ParentTheme = attribute == null ? string.Empty : attribute.Value;
        }

        public string ParentTheme { get; protected set; }
    }
}
