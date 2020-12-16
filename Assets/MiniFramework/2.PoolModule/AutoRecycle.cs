using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class AutoRecycle : MonoBehaviour
    {
        public float delay;
        private void OnEnable()
        {
            StartCoroutine(Recycle());
        }

        private IEnumerator Recycle()
        {
            yield return new WaitForSeconds(delay);
            PoolManager.Instance.Recycle(gameObject);
        }
    }
}