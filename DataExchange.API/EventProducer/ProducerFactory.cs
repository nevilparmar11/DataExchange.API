using Confluent.Kafka;
using DataExchange.API.Config;
using DataExchange.API.EventProducer.Interface;
using Microsoft.Extensions.Logging;

namespace DataExchange.API.EventProducer
{
    public class ProducerFactory : IProducerFactory
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly ILogger<ProducerFactory> _logger;

        public ProducerFactory(KafkaSettings kafkaSettings, ILogger<ProducerFactory> logger)
        {
            _kafkaSettings = kafkaSettings;
            _logger = logger;
        }

        public IProducer<string, string> CreateInstance()
        {
            return new ProducerBuilder<string, string>(_kafkaSettings.ProducerConfig)
                .SetErrorHandler((_, error) =>
                {
                    if (error.IsFatal)
                    {
                        _logger.LogCritical("Fatal ProducerBuilder error {ErrorReason}", error.Reason);
                    }

                    if (error.IsError || error.IsBrokerError || error.IsLocalError)
                    {
                        _logger.LogError("ProducerBuilder error {ErrorReason}", error.Reason);
                    }
                })
                .SetLogHandler((_, message) =>
                    {
                        switch (message.Level)
                        {
                            case SyslogLevel.Emergency:
                            case SyslogLevel.Alert:
                            case SyslogLevel.Critical:
                                _logger.LogCritical(message.Message);
                                break;
                            case SyslogLevel.Error:
                                _logger.LogError(message.Message);
                                break;
                            case SyslogLevel.Warning:
                            case SyslogLevel.Notice:
                                _logger.LogWarning(message.Message);
                                break;
                            case SyslogLevel.Info:
                                _logger.LogInformation(message.Message);
                                break;
                            case SyslogLevel.Debug:
                                _logger.LogTrace(message.Message);
                                break;
                            default:
                                break;
                        }
                    })
                .Build();
        }
    }
}