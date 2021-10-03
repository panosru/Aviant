namespace Aviant.DDD.Application.Services
{
    using System;
    using System.Collections.Generic;

    internal interface IJsonEventDeserializerCache
    {
        private static readonly IDictionary<string, Type> Cache = new Dictionary<string, Type>();

        public void Add(string key, Type type) =>
            Cache.Add(new KeyValuePair<string, Type>(key, type));

        public bool Exists(string key) =>
            Cache.ContainsKey(key);

        public void Remove(string key) =>
            Cache.Remove(key);

        public Type Get(string key) =>
            Cache[key];
    }
}
