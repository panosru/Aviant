namespace Aviant.Core.Reflection;

public interface ITypeFinder
{
    Type[] Find(Func<Type, bool> predicate);

    Type[] FindAll();
}
