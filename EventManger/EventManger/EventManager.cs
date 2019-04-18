using EventManger.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManger {
    public delegate void EventDelegate(Event gameEvent);
    public enum EventHandlerPriority {
        High,
        Default,
        Low,
        Count
    }
    public static class EventManager {

        private static Dictionary<Type, EventDelegate>[] eventHandlers = new Dictionary<Type, EventDelegate>[(int)EventHandlerPriority.Count];
        private static Queue<Event> currentEvents = new Queue<Event>();
        private static Queue<Event> newEvents = new Queue<Event>();

        private static List<Event> abortedEvents = new List<Event>();

        private static List<KeyValuePair<Type, EventDelegate>>[] eventHandlersToAdd = new List<KeyValuePair<Type, EventDelegate>>[(int)EventHandlerPriority.Count];
        private static List<KeyValuePair<Type, EventDelegate>> eventHandlersToRemove = new List<KeyValuePair<Type, EventDelegate>>();


        public static void Initialize() {
            InitializeColections();

        }

        private static void InitializeColections() {
            for (int i = 0; i < (int)EventHandlerPriority.Count; i++) {
                eventHandlers[i] = new Dictionary<Type, EventDelegate>();
                eventHandlersToAdd[i] = new List<KeyValuePair<Type, EventDelegate>>();
            }
            ClearEventQueues();
            eventHandlersToRemove.Clear();
        }

        private static void ClearEventQueues() {
            currentEvents.Clear();
            newEvents.Clear();
            abortedEvents.Clear();
        }

        public static void AddHandler(Type eventType, EventDelegate handler, EventHandlerPriority priority = EventHandlerPriority.Default) {
            if (!eventType.IsSubclassOf(typeof(Event))) {
                throw new InvalidEventTypeException(eventType);
            }
            eventHandlersToAdd[(int)priority].Add(new KeyValuePair<Type, EventDelegate>(eventType,handler));
        }
    }
}
