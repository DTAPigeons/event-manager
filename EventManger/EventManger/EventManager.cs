using EventMangement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMangement
{
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

        public static void AddEventHandler(Type eventType, EventDelegate handler, EventHandlerPriority priority = EventHandlerPriority.Default) {
            if (!eventType.IsSubclassOf(typeof(Event))) {
                throw new InvalidEventTypeException(eventType);
            }
            if(priority == EventHandlerPriority.Count)
            {
                throw new InvalidPriorityExeption(priority);
            }
            eventHandlersToAdd[(int)priority].Add(new KeyValuePair<Type, EventDelegate>(eventType,handler));
        }

        public static void RemoveEventHandler(Type eventType, EventDelegate handler) {
            if (!eventType.IsSubclassOf(typeof(Event))) {
                throw new InvalidEventTypeException(eventType);
            }
            eventHandlersToRemove.Add(new KeyValuePair<Type, EventDelegate>(eventType, handler));
        }

        public static void QueueEvent(Event newEvent)
        {
            newEvents.Enqueue(newEvent);
        }

        public static void AbortEvent(Event toAbort)
        {
            abortedEvents.Add(toAbort);
        }

        public static void Update()
        {
            RemoveHandlers();
            AddHandlers();
            TransferEvents();
            ProcessEvents();
        }

        private static void RemoveHandlers()
        {
            foreach(var pair in eventHandlersToRemove)
            {
                foreach(var priority in eventHandlers)
                {
                    if (priority.ContainsKey(pair.Key))
                    {
                        priority[pair.Key] -= pair.Value;
                    }
                }
            }
        }

        private static void AddHandlers()
        {
            for (int i = 0; i < (int)EventHandlerPriority.Count; i++)
            {
                foreach (var pair in eventHandlersToAdd[i])
                {
                    if (eventHandlers[i].ContainsKey(pair.Key))
                    {
                        eventHandlers[i][pair.Key] -= pair.Value;        //Makes sure the same event handler cannot be added twice to the same event type
                        eventHandlers[i][pair.Key] += pair.Value;
                    }
                    else
                    {
                        eventHandlers[i].Add(pair.Key, pair.Value);
                    }
                }
                eventHandlersToAdd[i].Clear();
            }            
        }

        private static void TransferEvents()
        {
            while (newEvents.Count > 0)
            {
                currentEvents.Enqueue(newEvents.Dequeue());
            }
        }

        private static void ProcessEvents()
        {
            Event eventToProcess = null;
            while (currentEvents.Count > 0)
            {
                eventToProcess = currentEvents.Dequeue();
                if(abortedEvents.Any(e=> e.GetType() == eventToProcess.GetType()))
                {
                    continue;
                }

                DispatchEvent(eventToProcess);
            }
            abortedEvents.Clear();
        }

        private static void DispatchEvent(Event @event)
        {
            DispatchEventToHandlerByPriority(@event, EventHandlerPriority.High);
            DispatchEventToHandlerByPriority(@event, EventHandlerPriority.Default);
            DispatchEventToHandlerByPriority(@event, EventHandlerPriority.Low);
        }

        private static void DispatchEventToHandlerByPriority(Event @event, EventHandlerPriority priority)
        {
            if (eventHandlers[(int)priority].ContainsKey(@event.GetType()))
            {
                eventHandlers[(int)priority][@event.GetType()]?.Invoke(@event);
            }
        }
    }
}
