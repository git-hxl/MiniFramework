using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class DelayNode
    {
        public IEnumerator Delay(float delay)
        {
            yield return new WaitForSeconds(delay);
        }
    }
}
