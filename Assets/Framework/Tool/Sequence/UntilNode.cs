using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class UntilNode
    {
        public IEnumerator Until(Func<bool> condition)
        {
            yield return new WaitUntil(condition);
        }
    }
}
