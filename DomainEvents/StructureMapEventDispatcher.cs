using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEvents
{
    public class StructureMapEventDispatcher : IEventDispatcher
    {
        public void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IEvent
        {
            foreach (var handler in ObjectFactory.GetAllInstances<IEventHandler<TEvent>>())
            {
                handler.Handle(eventToDispatch);
            }
        }
    }
}