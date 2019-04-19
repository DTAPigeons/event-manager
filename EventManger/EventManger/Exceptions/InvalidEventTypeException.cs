using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangement.Exceptions {
     public class InvalidEventTypeException: EventManagerExeption {
        private Type invalidType;
        public InvalidEventTypeException(Type invalidType) {
            this.invalidType = invalidType;
        }

        public override string Message => $"The type {invalidType} is not a valid Event type!";
    }
}
