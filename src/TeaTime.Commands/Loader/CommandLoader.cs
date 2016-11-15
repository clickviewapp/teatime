namespace TeaTime.Commands.Loader
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Extensions.DependencyModel;
    using System.Linq;
    using Core;

    public static class CommandLoader
    {
        private static readonly Type CommandClass = typeof(Command);

        public static IEnumerable<Type> GetCommands()
        {
            var assemblies = GetAssemblies();
            var commands = new List<Type>();

            foreach (var assemby in assemblies)
                foreach (var t in assemby.GetTypes())
                {
                    if (IsValidCommand(t))
                    {
                        commands.Add(t);
                    }
                }

            return commands;
        }

        public static bool IsValidCommand(Type type)
        {
            return IsValidCommand(type.GetTypeInfo());
        }
        
        public static bool IsValidCommand(TypeInfo typeInfo)
        {
            return typeInfo.IsClass && !typeInfo.IsAbstract && typeInfo.IsSubclassOf(CommandClass);
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                if (IsCandidateCompilationLibrary(library))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }

        private static bool IsCandidateCompilationLibrary(Library compilationLibrary)
        {
            return compilationLibrary.Name.StartsWith("TeaTime")
                || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith("TeaTime"));
        }
    }
}
