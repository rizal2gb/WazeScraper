using Autofac;
using System;
using System.Linq;
using System.Reflection;
using WazeScraper.Utils;
using IContainer = Autofac.IContainer;

namespace WazeScraper
{
    public static class AutofacInitializer
    {
        private static IContainer _container;

        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            var loadedAssemblies = AssemblyLocator.LoadMissingAssemblies();
            RegisterUndefinedScope(loadedAssemblies, builder);
            RegisterSingleInstance(loadedAssemblies, builder);
            RegisterAsInterfaceSingleInstance(loadedAssemblies, builder);
            _container = builder.Build();
        }

        public static T GetInstance<T>() where T : class => _container?.Resolve<T>();

        public static object GetInstanceOf(Type t) => _container?.Resolve(t);

        /// <summary>
        /// These classes are either framework-generated or require special treatment - like unique registration options.
        /// </summary>
        private static void RegisterUndefinedScope(Assembly[] loadedAssemblies, ContainerBuilder builder)
        {
            // e.g. builder.RegisterType<PersistedAuthenticator>().As<IPersistedAuthenticator>().SingleInstance();
        }

        private static void RegisterSingleInstance(Assembly[] loadedAssemblies, ContainerBuilder builder)
        {
            loadedAssemblies.ToList().ForEach(a => a.GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.GetCustomAttributes(typeof(AppScope), false)
                    .Any(att => ((AppScope)att).Scope == Scope.SingleInstance &&
                                !((AppScope)att).RegisterAsInterface)).ToList()
                .ForEach(type => builder.RegisterType(type).SingleInstance()));
        }

        private static void RegisterAsInterfaceSingleInstance(Assembly[] loadedAssemblies, ContainerBuilder builder)
        {
            loadedAssemblies.ToList().ForEach(a => a.GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.GetCustomAttributes(typeof(AppScope), false)
                    .Any(att => ((AppScope)att).Scope == Scope.SingleInstance && ((AppScope)att).RegisterAsInterface)).ToList()
                .ForEach(type => builder.RegisterType(type).SingleInstance().As((Type[])type.GetInterfaces())));
        }
    }
}
