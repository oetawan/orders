using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEvents
{
    public class ActionEventDispatcher : IEventDispatcher
    {
        private readonly IDictionary<Type, Delegate> handlers;

        public ActionEventDispatcher()
        {
            handlers = new Dictionary<Type, Delegate>();
        }

        public void Register<TEvent>(Action<TEvent> eventAction) where TEvent : IEvent
        {
            // assuming you'll only register once per test
            handlers.Add(typeof(TEvent), eventAction);
        }

        public void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IEvent
        {
            if (!handlers.ContainsKey(typeof(TEvent))) return;

            var handler = (Action<TEvent>)handlers[typeof(TEvent)];

            handler.Invoke(eventToDispatch);
        }
    }
}