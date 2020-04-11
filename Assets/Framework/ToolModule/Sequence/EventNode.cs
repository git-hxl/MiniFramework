using System;
using System.Collections;
namespace MiniFramework
{
    public class EventNode
    {
        public IEnumerator Event(Action action)
        {
            action.Invoke();
            yield return null;
        }
    }
}
