namespace Aviant.DDD.Core.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal static class AssemblyHelper
    {
        public static List<Assembly> GetAllAssembliesInFolder(string folderPath, SearchOption searchOption)
        {
            IEnumerable<string> assemblyFiles = Directory
               .EnumerateFiles(folderPath, "*.*", searchOption)
               .Where(
                    s =>
                        s.EndsWith(".dll", StringComparison.Ordinal)
                     || s.EndsWith(".exe", StringComparison.Ordinal));

            return assemblyFiles.Select(
                    Assembly.LoadFile
                )
               .ToList();
        }
    }
}