using System;

namespace  MiniFramework
{
    public interface IChain {
        IChain Delay(float seconds);
        IChain Event(Action action);
        IChain Until(Func<bool> condition);
        void Execute();
    }
}