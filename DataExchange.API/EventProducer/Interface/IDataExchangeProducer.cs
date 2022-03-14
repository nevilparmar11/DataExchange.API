using DataExchange.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExchange.API.EventProducer.Interface
{
    public interface IDataExchangeProducer
    {
        Task ProduceAsync(DataExchangeEvent dataExchangeEvent);
    }
}
