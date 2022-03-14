#nullable enable
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataExchange.API.Serialization
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            if (target == null)
            {
                throw new DataExchangeEventConverterException($"Could not create an instance of type: {target}");
            }

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        protected abstract T Create(Type objectType, JObject jObject);
    }
}