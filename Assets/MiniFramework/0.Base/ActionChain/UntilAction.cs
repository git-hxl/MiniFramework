using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class UntilAction : IChain
    {
        public override IChain Until(Func<bool> condition)
        {
            Nodes.Add(conditionCoroutine(condition));
            return this;
        }
        private IEnumerator conditionCoroutine(Func<bool> condition)
        {
            yield return new WaitUntil(condition);
        }
    }
}