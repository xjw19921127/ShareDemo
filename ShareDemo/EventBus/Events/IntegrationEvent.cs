using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Events
{
    public class IntegrationEvent
    {
        [JsonProperty]
        public string Id { get; private set; }
        [JsonProperty]
        public DateTime CreateTime { get; private set; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }

        [JsonConstructor]
        public IntegrationEvent(string id, DateTime createTime)
        {
            Id = id;
            CreateTime = createTime;
        }
    }
}
