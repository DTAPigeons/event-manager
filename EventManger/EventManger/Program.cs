using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangement {
    class Program {
        static void Main(string[] args) {
            EventManager.Initialize();
            EventManager.AddEventHandler(typeof(TestEventOne), HandleAnyEvent);
            EventManager.AddEventHandler(typeof(TestEventOne), HandleTestEventOne, EventHandlerPriority.High);
            EventManager.AddEventHandler(typeof(TestEventOne), HandleTestEventTwo, EventHandlerPriority.Low);

            EventManager.QueueEvent(new TestEventOne("Hi ya!"));

            EventManager.AddEventHandler(typeof(TestEventTwo), HandleAnyEvent, EventHandlerPriority.High);
            EventManager.AddEventHandler(typeof(TestEventTwo), HandleTestEventOne);
            EventManager.AddEventHandler(typeof(TestEventTwo), HandleTestEventTwo);

            EventManager.QueueEvent(new TestEventTwo("Supp?"));

            EventManager.Update();
            EventManager.Update();

            Console.ReadLine();
        }

        static void HandleAnyEvent(Event @event)
        {
            if (@event == null)
            {
                Console.WriteLine("The event is null");
            }
            else
            {
                Console.WriteLine("Generic event hadling!");
            }
        }

        static void HandleTestEventOne(Event @event)
        {
            TestEventOne castEvent = @event as TestEventOne;
            if(castEvent == null)
            {
                Console.WriteLine("Wrong Event!");
            }
            else
            {
                Console.WriteLine(castEvent);
            }
        }

        static void HandleTestEventTwo(Event @event)
        {
            TestEventTwo castEvent = @event as TestEventTwo;
            if (castEvent == null)
            {
                Console.WriteLine("Wrong Event!");
            }
            else
            {
                Console.WriteLine(castEvent);
            }
        }
    }


}
