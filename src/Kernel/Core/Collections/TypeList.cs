using System.Collections;
using System.Reflection;

namespace Aviant.Core.Collections;

/// <inheritdoc cref="Aviant.Core.Collections.ITypeList" />
/// <summary>
/// A shortcut for <see cref="T:Aviant.Core.Collections.TypeList`1" /> to use object as base type.
/// </summary>
public class TypeList : TypeList<object>, ITypeList;

/// <inheritdoc />
/// <summary>
/// Extends <see cref="T:System.Collections.Generic.List`1" /> to add restriction a specific base type.
/// </summary>
/// <typeparam name="TBaseType">Base Type of <see cref="T:System.Type" />s in this list</typeparam>
public class TypeList<TBaseType> : ITypeList<TBaseType>
{
    /// <inheritdoc />
    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count => _typeList.Count;

    /// <inheritdoc />
    /// <summary>
    /// Gets a value indicating whether this instance is read only.
    /// </summary>
    /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
    public bool IsReadOnly => false;

    /// <inheritdoc />
    /// <summary>
    /// Gets or sets the <see cref="T:System.Type" /> at the specified index.
    /// </summary>
    /// <param name="index">Index.</param>
    public Type this[int index]
    {
        get => _typeList[index];
        set
        {
            CheckType(value);
            _typeList[index] = value;
        }
    }

    private readonly List<Type> _typeList;

    /// <summary>
    /// Creates a new <see cref="TypeList{T}"/> object.
    /// </summary>
    public TypeList() => _typeList = new List<Type>();

    /// <inheritdoc/>
    public void Add<T>()
        where T : TBaseType =>
        _typeList.Add(typeof(T));

    /// <inheritdoc/>
    public void Add(Type item)
    {
        CheckType(item);
        _typeList.Add(item);
    }

    /// <inheritdoc/>
    public void Insert(int index, Type item) =>
        _typeList.Insert(index, item);

    /// <inheritdoc/>
    public int IndexOf(Type item) => _typeList.IndexOf(item);

    /// <inheritdoc/>
    public bool Contains<T>()
        where T : TBaseType => Contains(typeof(T));

    /// <inheritdoc/>
    public bool Contains(Type item) => _typeList.Contains(item);

    /// <inheritdoc/>
    public void Remove<T>()
        where T : TBaseType =>
        _typeList.Remove(typeof(T));

    /// <inheritdoc/>
    public bool Remove(Type item) => _typeList.Remove(item);

    /// <inheritdoc/>
    public void RemoveAt(int index) =>
        _typeList.RemoveAt(index);

    /// <inheritdoc/>
    public void Clear() => _typeList.Clear();

    /// <inheritdoc/>
    public void CopyTo(Type[] array, int arrayIndex) =>
        _typeList.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public IEnumerator<Type> GetEnumerator() => _typeList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _typeList.GetEnumerator();

    private static void CheckType(Type item)
    {
        if (!typeof(TBaseType).GetTypeInfo().IsAssignableFrom(item))
            throw new ArgumentException(
                "Given item is not type of " + typeof(TBaseType).AssemblyQualifiedName,
                nameof(item));
    }
}
