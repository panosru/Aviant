namespace Aviant.DDD.Core.Collections
{
    using System;
    using System.Collections.Generic;

    /// <inheritdoc />
    /// <summary>
    /// A shortcut for <see cref="T:Aviant.DDD.Core.Collections.ITypeList`1" /> to use object as base type.
    /// </summary>
    public interface ITypeList : ITypeList<object>
    { }

    /// <inheritdoc />
    /// <summary>
    /// Extends <see cref="T:System.Collections.Generic.IList`1" /> to add restriction a specific base type.
    /// </summary>
    /// <typeparam name="TBaseType">Base Type of <see cref="T:System.Type" />s in this list</typeparam>
    public interface ITypeList<in TBaseType> : IList<Type>
    {
        /// <summary>
        /// Adds a type to list.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void Add<T>()
            where T : TBaseType;

        /// <summary>
        /// Checks if a type exists in the list.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        bool Contains<T>()
            where T : TBaseType;

        /// <summary>
        /// Removes a type from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Remove<T>()
            where T : TBaseType;
    }
}