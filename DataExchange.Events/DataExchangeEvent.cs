using DataExchange.Events.Employee;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DataExchange.Events
{
    [KnownType(typeof(EmployeeCreated))]
    [KnownType(typeof(EmployeeUpdated))]
    [KnownType(typeof(EmployeeDeleted))]

    public abstract class DataExchangeEvent
    {
        public string EventName => GetType().Name;

        /// <summary>
        /// Unique Identifier for the event.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Environment generating the event eg. Cloud/App/Dev/QA/Sandbox etc..
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Source application generating the event eg, ENT/PROE etc...
        /// </summary>
        public string Initializer { get; set; }

        /// <summary>
        /// Time the event is generated.
        /// </summary>
        public DateTime Timestamp { get; set; }

        public abstract string GetPartitionKey();
    }
}
