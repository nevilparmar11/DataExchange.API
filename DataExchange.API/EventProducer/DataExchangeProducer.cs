using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using DataExchange.API.EventProducer.Interface;
using DataExchange.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DataExchange.API.EventProducer
{
    public class DataExchangeProducer : IDataExchangeProducer
    {
        private readonly Dictionary<string, string> _eventNamesToTopicsMapping;
        private readonly ILogger<DataExchangeProducer> _logger;
        private readonly IProducer<string, string> _producer;

        public DataExchangeProducer(Dictionary<string, string> eventNamesToTopicsMapping, ILogger<DataExchangeProducer> logger, IProducerFactory producerFactory)
        {
            _eventNamesToTopicsMapping = eventNamesToTopicsMapping;
            _logger = logger;
            _producer = producerFactory.CreateInstance();
        }

        public async Task ProduceAsync(DataExchangeEvent dataExchangeEvent)
        {
            var topic = _eventNamesToTopicsMapping[dataExchangeEvent.EventName];

            var message = new Message<string, string>
            {
                Key = dataExchangeEvent.GetPartitionKey() ?? Guid.NewGuid().ToString(),
                Value = JsonConvert.SerializeObject(
                    dataExchangeEvent,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                Timestamp = Timestamp.Default,
            };

            var result = await _producer.ProduceAsync(topic, message);

            _logger.LogInformation(
                        "Produced {EventName} with Key:{Key} to {Topic}:{Partition}:{Offset}",
                        dataExchangeEvent.EventName,
                        result.Key,
                        result.Topic,
                        result.Partition.Value,
                        result.Offset.Value);
        }
    }
}
