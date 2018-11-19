namespace Atanet.WebApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyUtilities
    {
        private const string AtanetAssemblyIdentifier = "Atanet";

        private static Lazy<List<Assembly>> atanetAssemblies = new Lazy<List<Assembly>>(
            () => AssemblyUtilities.IterateAssemblies().ToList());

        public static IEnumerable<Assembly> GetAtanetAssemblies() =>
            AssemblyUtilities.atanetAssemblies.Value;

        private static IEnumerable<Assembly> IterateAssemblies()
        {
            yield return Assembly.GetExecutingAssembly();
            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.Name.Contains(AssemblyUtilities.AtanetAssemblyIdentifier))
                {
                    var loaded = Assembly.Load(assembly);
                    yield return loaded;
                }
            }
        }
    }
}
