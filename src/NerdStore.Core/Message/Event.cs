using MediatR;
using System;

namespace NerdStore.Core.Message
{
    public class Event : Message, INotification
    {
        public DateTime TimeStamp { get; private set; }

        public Event()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
