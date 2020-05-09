using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerApp.Event
{
    public class TestIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; }

        public TestIntegrationEvent(string userId)
        {
            UserId = userId;
        }

        //public TestIntegrationEvent(string userId, string id, DateTime createTime) : base(id, createTime) 
        //{
        //    UserId = userId;
        //}
    }
}
