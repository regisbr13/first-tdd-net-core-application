using FluentValidation.Results;
using System;

namespace NerdStore.Core.Message
{
    public abstract class Command : Message
    {
        public DateTime TimeStamp { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public Command()
        {
            TimeStamp = DateTime.Now;
        }

        public abstract bool IsValid();
    }
}
