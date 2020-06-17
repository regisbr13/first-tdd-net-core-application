using FluentValidation.Results;
using MediatR;
using System;

namespace NerdStore.Core.Message
{
    public abstract class Command : Message, IRequest<bool>
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
