using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.Message
{
    public abstract class Message
    {
        public string MessageType { get; set; }
        public Guid AgregateId { get; set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
