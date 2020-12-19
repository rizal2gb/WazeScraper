using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace WazeScraper.Utils
{
    public static class AssemblyLocator
    {
        private const string AssemblyPrefixFilter = @"WazeScraper";

        public static Assembly[] LoadMissingAssemblies()
        {
            AssemblyName[] names = FindReferencedAssemblies(AssemblyPrefixFilter);
            Assembly[] assemblies = names.Select(asm => FindLoadAssembly(AppDomain.CurrentDomain, asm)).ToArray();

            return assemblies;
        }

        /// <summary>
        /// Find all referenced assemblies matching the specified filter string.
        /// </summary>
        /// <param name="filter">Name filter string.</param>
        /// <returns>Array of assembly names</returns>
        public static AssemblyName[] FindReferencedAssemblies(string filter)
        {
            List<AssemblyName> assemblyNames = new List<AssemblyName>();

            RecurseFilterAssemblies(assemblyNames, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly().GetName(), filter);

            return assemblyNames.ToArray();
        }

        /// <summary>
        /// Recursively try loading and inspecting all assemblies matching the filter string,
        /// then returning a combined list of unique names matching the filter.
        /// </summary>
        /// <param name="foundAssemblyNames">List to put discovered assembly names into.</param>
        /// <param name="domain">AppDomain to use for loading (can be different than current AppDomain).</param>
        /// <param name="assemblyName">Name of assembly to load and inspect.</param>
        /// <param name="filter">Name filter string.</param>
        private static void RecurseFilterAssemblies(List<AssemblyName> foundAssemblyNames, AppDomain domain, AssemblyName assemblyName, string filter)
        {
            bool ExistsInList(AssemblyName asmToCheck)
            {
                foreach (var asmInList in foundAssemblyNames)
                    if (asmToCheck.FullName == asmInList.FullName)
                        return true;

                return false;
            }

            if (ExistsInList(assemblyName))
                return;

            foundAssemblyNames.Add(assemblyName);

            Assembly assembly;
            try
            {
                assembly = FindLoadAssembly(domain, assemblyName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return;
            }

            foreach (AssemblyName asmName in assembly.GetReferencedAssemblies())
            {
                if (!asmName.FullName.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                RecurseFilterAssemblies(foundAssemblyNames, domain, asmName, filter);
            }
        }

        /// <summary>
        /// Try locating assembly in the loaded assemblies name, and only if not found -- load it.
        /// </summary>
        /// <param name="domain">AppDomain to load the assembly into.</param>
        /// <param name="assemblyName">AssemblyName of the desired assembly.</param>
        /// <returns>Assembly</returns>
        private static Assembly FindLoadAssembly(AppDomain domain, AssemblyName assemblyName)
        {
            foreach (var assembly in domain.GetAssemblies())
                if (assembly.FullName == assemblyName.FullName)
                    return assembly;

            return domain.Load(assemblyName);
        }
    }
}
