using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEvents
{
    public static class DomainEvent
    {
        public static IEventDispatcher Dispatcher { get; set; }

        public static void Raise<TEvent>(TEvent eventToRaise) where TEvent : IEvent
        {
            Dispatcher.Dispatch(eventToRaise);
        }
    }
}