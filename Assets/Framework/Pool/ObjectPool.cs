using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Resource;
namespace MiniFramework.Pool
{
    public class ObjectPool : MonoSingleton<ObjectPool>, IObjectPool
    {
        private Dictionary<string, Stack<GameObject>> cachedObjects = new Dictionary<string, Stack<GameObject>>();
        public int Count(string name)
        {
            return cachedObjects.ContainsKey(name) ? cachedObjects[name].Count : 0;
        }
        private GameObject PopFromPool(string name)
        {
            GameObject obj = null;
            if (Count(name) > 0)
            {
                obj = cachedObjects[name].Pop();
                if (obj == null)
                {
                    obj = PopFromPool(name);
                }
            }
            return obj;
        }
        //分配
        public GameObject Allocate(string name)
        {
            GameObject obj = PopFromPool(name);
            if (obj == null)
            {
                GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>(name);
                if (asset != null)
                {
                    obj = Instantiate(asset);
                    obj.name += "(Pool)";
                }
            }
            if (obj == null)
                Debug.LogError("从缓存池中获取对象失败:" + name);
            else
            {
                ObjectPoolable objectPoolable = obj.GetComponent<ObjectPoolable>();
                if (objectPoolable == null)
                {
                    objectPoolable = obj.AddComponent<ObjectPoolable>();
                }
                objectPoolable.PoolKey = name;
                objectPoolable.IsRecycled = false;
                obj.SetActive(true);
            }
            return obj;
        }
        //回收
        public bool Recycle(GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }
            ObjectPoolable objectPoolable = obj.GetComponent<ObjectPoolable>();
            if (objectPoolable == null || objectPoolable.IsRecycled)
            {
                return false;
            }
            obj.SetActive(false);
            objectPoolable.IsRecycled = true;

            Stack<GameObject> values;
            if (!cachedObjects.TryGetValue(objectPoolable.PoolKey, out values))
            {
                values = new Stack<GameObject>();
                cachedObjects.Add(objectPoolable.PoolKey, values);
            }
            values.Push(obj);
            return true;
        }
    }

}