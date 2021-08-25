namespace Aviant.DDD.Core.Reflection.Extensions
{
    using System.IO;
    using System.Reflection;

    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Gets directory path of given assembly or returns null if can not find.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static string? GetDirectoryPathOrNull(this Assembly assembly)
        {
            var location = assembly.Location;

            var directory = new FileInfo(location).Directory;

            return directory?.FullName;
        }
    }
}