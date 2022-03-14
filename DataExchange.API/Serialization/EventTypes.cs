#nullable enable
using DataExchange.Events;
using System;
using System.Linq;

namespace DataExchange.API.Serialization
{
    public static class EventTypes
    {
        public static DataExchangeEvent CreateInstanceBinder(string eventName)
        {
            var parentType = typeof(DataExchangeEvent);
            var eventClasses = parentType.Assembly.GetTypes().Where(t => t.BaseType == parentType);

            var type = Type.GetType(
                eventClasses.FirstOrDefault(x => x.Name == eventName)?.AssemblyQualifiedName ??
                string.Empty);

            DataExchangeEventConverterException dexException;

            if (type is null)
            {
                dexException = new DataExchangeEventConverterException(
                    $"An event class: {eventName} does not exist, EventName: {eventName} cannot be mapped");

                dexException.Data.Add("EventType", eventName);
                throw dexException;
            }

            dynamic? eventTypeInstance = Activator.CreateInstance(type);
            dexException = new DataExchangeEventConverterException($"Could not create an instance of type: {type}");
            dexException.Data.Add("EventType", eventName);
            return eventTypeInstance ??
                   throw dexException;
        }
    }
}