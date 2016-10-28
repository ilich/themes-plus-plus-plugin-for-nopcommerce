using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Development.ThemesPlusPlus
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<ThemePlusPlusProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
        }
    }
}
