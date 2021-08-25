namespace Aviant.DDD.Core.Reflection
{
    using System;

    public interface ITypeFinder
    {
        Type[] Find(Func<Type, bool> predicate);

        Type[] FindAll();
    }
}