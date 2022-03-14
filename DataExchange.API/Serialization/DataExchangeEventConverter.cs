#nullable enable
using System;
using DataExchange.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataExchange.API.Serialization
{
    public class DataExchangeEventConverter : JsonCreationConverter<DataExchangeEvent>
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override DataExchangeEvent Create(Type objectType, JObject jObject)
        {
            const string fieldName = "EventName";
            const string fieldNameCamelCase = "eventName";

            if (!FieldExists(fieldName, jObject) && !FieldExists(fieldNameCamelCase, jObject))
            {
                throw new DataExchangeEventConverterException("Field name EventName does not exist");
            }

            var eventName = jObject[fieldName]?.ToString() ?? jObject[fieldNameCamelCase]?.ToString();
            if (string.IsNullOrWhiteSpace(eventName))
            {
                var dexException = new DataExchangeEventConverterException("Field name EventName does not exist");
                dexException.Data.Add("EventType", eventName);

                throw dexException;
            }

            var eventTypeInstance = EventTypes.CreateInstanceBinder(eventName);
            return eventTypeInstance;
        }

        private static bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}