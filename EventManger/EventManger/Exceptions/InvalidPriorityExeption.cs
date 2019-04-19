using EventMangement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangement.Exceptions
{
    class InvalidPriorityExeption : EventManagerExeption
    {
        private EventHandlerPriority priority;
        public InvalidPriorityExeption(EventHandlerPriority priority)
        {
            this.priority = priority;
        }

        public override string Message => $"The priority {priority} is not a valid Event Handler priority!";
    }
}
