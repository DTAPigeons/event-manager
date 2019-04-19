using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangement
{
    class TestEventTwo : Event
    {
        private string message;
        public TestEventTwo(string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return this.GetType() + ": " + message;
        }
    }
}
