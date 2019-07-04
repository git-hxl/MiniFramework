using System;
namespace MiniFramework
{
    public class IChain : Sequence
    {
        public virtual IChain Delay(float seconds) { return null; }
        public virtual IChain Event(Action action) { return null; }
        public virtual IChain Until(Func<bool> condition) { return null; }
        public virtual void Excute() { }
    }
}