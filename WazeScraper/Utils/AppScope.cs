using System;

namespace WazeScraper.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppScope : Attribute
    {
        public Scope Scope;
        public bool RegisterAsInterface;

        public AppScope(Scope scope, bool registerAsInterface = false)
        {
            Scope = scope;
            RegisterAsInterface = registerAsInterface;
        }
    }

    public enum Scope
    {
        InstancePerLifetime,
        SingleInstance
    }
}
