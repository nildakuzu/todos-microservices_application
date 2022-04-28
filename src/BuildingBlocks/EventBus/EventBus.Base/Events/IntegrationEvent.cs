using Newtonsoft.Json;
using System;

namespace EventBus.Base.Events
{
    public class IntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreatedDateTime { get; private set; }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdDateTime)
        {
            Id = id;
            CreatedDateTime = createdDateTime;
        }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedDateTime = DateTime.Now;
        }
    }
}
