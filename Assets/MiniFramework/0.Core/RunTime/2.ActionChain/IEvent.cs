using System;
namespace MiniFramework
{
    public interface IEvent
    {
        Sequence Event(Action handler);
    }
}

