using System;

namespace DataExchange.API.Serialization
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class DataExchangeEventConverterException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public DataExchangeEventConverterException()
        {
        }

        public DataExchangeEventConverterException(string message)
            : base(message)
        {
        }

        public DataExchangeEventConverterException(string message, Exception innException)
            : base(message, innException)
        {
        }
    }
}