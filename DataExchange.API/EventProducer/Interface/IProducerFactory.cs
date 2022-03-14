using Confluent.Kafka;

namespace DataExchange.API.EventProducer.Interface
{
    public interface IProducerFactory
    {
        IProducer<string, string> CreateInstance();
    }
}