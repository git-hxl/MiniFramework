using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    class DelayAction : IChain
    {
        public override IChain Delay(float seconds)
        {
            Nodes.Add(delayCoroutine(seconds));
            return this;
        }
        private IEnumerator delayCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}