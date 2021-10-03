namespace Aviant.DDD.Core.Reflection.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static Assembly GetAssembly(this Type type) => type.GetTypeInfo().Assembly;

        public static MethodInfo GetMethod(
            this Type type,
            string    methodName,
            int       pParametersCount       = 0,
            int       pGenericArgumentsCount = 0)
        {
            return type
               .GetMethods()
               .Where(m => m.Name == methodName)
               .AsEnumerable()
               .Select(
                    m => new
                    {
                        Method = m,
                        Params = m.GetParameters(),
                        Args   = m.GetGenericArguments()
                    })
               .Where(
                    x => x.Params.Length == pParametersCount
                      && x.Args.Length   == pGenericArgumentsCount
                )
               .Select(x => x.Method)
               .First();
        }
    }
}