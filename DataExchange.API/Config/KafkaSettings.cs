using Confluent.Kafka;

namespace DataExchange.API.Config
{
    public class KafkaSettings
    {
        public ProducerConfig ProducerConfig { get; set; }
    }
}
