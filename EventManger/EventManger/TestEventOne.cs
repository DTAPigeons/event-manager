using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangement
{
    class TestEventOne: Event
    {
        private string message;
        public TestEventOne(string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return this.GetType() + ": " + message;
        }
    }
}
