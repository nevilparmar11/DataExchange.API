using DataExchange.API.EventProducer.Interface;
using DataExchange.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExchange.API.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IDataExchangeProducer _producer;

        public EventController(ILogger<EventController> logger, IDataExchangeProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> Post(DataExchangeEvent dataExchangeEvent)
        {
            HttpContext.Items.Add("EventType", dataExchangeEvent.EventName);

            _logger.LogInformation(
                       "Processing event {EventName}:{EventId}:{Environment}",
                       dataExchangeEvent.EventName,
                       dataExchangeEvent.EventId,
                       dataExchangeEvent.Environment);

            await _producer.ProduceAsync(dataExchangeEvent);

            return Ok();
        }

    }
}
