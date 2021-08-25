namespace Aviant.DDD.Core.Json
{
    using System;
    using System.Data;
    using Newtonsoft.Json;

    /// <summary>
    ///     Defines helper methods to work with JSON.
    /// </summary>
    public static class JsonSerializationHelper
    {
        private const char TypeSeparator = '|';

        /// <summary>
        ///     Serializes an object with a type information included.
        ///     So, it can be deserialized using <see cref="DeserializeWithType" /> method later.
        /// </summary>
        public static string SerializeWithType(object obj) => SerializeWithType(obj, obj.GetType());

        /// <summary>
        ///     Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string Serialize(object obj) => obj.ToJsonString();

        /// <summary>
        ///     Serializes an object with a type information included.
        ///     So, it can be deserialized using <see cref="DeserializeWithType" /> method later.
        /// </summary>
        public static string SerializeWithType(object obj, Type type)
        {
            string serialized = obj.ToJsonString();

            return $"{type.AssemblyQualifiedName}{TypeSeparator}{serialized}";
        }

        /// <summary>
        ///     Deserializes an object serialized with <see cref="SerializeWithType(object)" /> methods.
        /// </summary>
        public static T DeserializeWithType<T>(string serializedObj) => (T)DeserializeWithType(serializedObj);

        /// <summary>
        ///     Deserializes an object serialized with <see cref="SerializeWithType(object)" /> methods.
        /// </summary>
        public static object DeserializeWithType(string serializedObj)
        {
            var    typeSeparatorIndex = serializedObj.IndexOf(TypeSeparator);
            Type   type = Type.GetType(serializedObj[..typeSeparatorIndex]) ?? throw new NoNullAllowedException();
            string serialized = serializedObj[(typeSeparatorIndex + 1)..];

            var options = new JsonSerializerSettings();
            options.Converters.Insert(0, new DateTimeConverter());

            return JsonConvert.DeserializeObject(serialized, type, options) ?? throw new NoNullAllowedException();
        }

        /// <summary>
        ///     Deserializes the specified serialized object.
        /// </summary>
        /// <param name="serializedObj">The serialized object.</param>
        /// <returns></returns>
        public static object Deserialize(string serializedObj)
        {
            var options = new JsonSerializerSettings();
            options.Converters.Insert(0, new DateTimeConverter());

            return JsonConvert.DeserializeObject(serializedObj, options) ?? throw new NoNullAllowedException();
        }
    }
}