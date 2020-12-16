using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        private Dictionary<string, Stack<GameObject>> poolObjects = new Dictionary<string, Stack<GameObject>>();
        private List<GameObject> allocatedObjets = new List<GameObject>();
        /// <summary>
        /// 添加缓存对象
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="maxCount"></param>
        public void Add(string assetPath, int maxCount)
        {
            Stack<GameObject> objs;
            if (!poolObjects.TryGetValue(assetPath, out objs))
            {
                objs = new Stack<GameObject>();
                poolObjects.Add(assetPath, objs);
            }

            for (int i = objs.Count; i < maxCount; i++)
            {
                GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>(assetPath);
                if (asset == null)
                {
                    Debug.LogError("添加到缓存池失败:" + assetPath);
                    return;
                }
                GameObject newObj = Instantiate(asset);
                Poolable poolable = newObj.GetComponent<Poolable>();
                if (poolable == null)
                {
                    poolable = newObj.AddComponent<Poolable>();
                }
                poolable.poolKey = assetPath;
                Recycle(newObj);
            }
        }

        /// <summary>
        /// 分配/创建对象
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public GameObject Allocate(string assetPath, bool isActive = true)
        {
            GameObject obj = Pop(assetPath);
            if (obj == null)
            {
                GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>(assetPath);
                if (asset != null)
                {
                    obj = Instantiate(asset);
                }
            }
            if (obj != null)
            {
                Poolable poolable = obj.GetComponent<Poolable>();
                if (poolable == null)
                {
                    poolable = obj.AddComponent<Poolable>();
                }
                poolable.poolKey = assetPath;
                poolable.IsRecycled = false;
                obj.SetActive(isActive);
                allocatedObjets.Add(obj);
                return obj;
            }
            Debug.LogError("从缓存池中获取对象失败:" + assetPath);
            return null;
        }
        /// <summary>
        /// 回收已分配对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Recycle(GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }
            Poolable poolable = obj.GetComponent<Poolable>();
            if (poolable == null || poolable.IsRecycled)
            {
                return false;
            }
            obj.SetActive(false);
            poolable.IsRecycled = true;
            Stack<GameObject> objs;
            if (!poolObjects.TryGetValue(poolable.poolKey, out objs))
            {
                objs = new Stack<GameObject>();
                poolObjects.Add(poolable.poolKey, objs);
            }
            objs.Push(obj);
            if (allocatedObjets.Contains(obj))
            {
                allocatedObjets.Remove(obj);
            }
            return true;
        }

        /// <summary>
        /// 获取已缓存对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private GameObject Pop(string name)
        {
            if (!poolObjects.ContainsKey(name))
            {
                return null;
            }
            GameObject obj = null;
            if (poolObjects[name].Count > 0)
            {
                obj = poolObjects[name].Pop();
                if (obj == null)
                {
                    obj = Pop(name);
                }
                else
                {
                    Poolable poolable = obj.GetComponent<Poolable>();
                    if (!poolable.IsRecycled)
                    {
                        obj = Pop(name);
                    }
                }
            }
            return obj;
        }

        public void Clear()
        {
            Debug.Log("清除缓存池对象");
            foreach (var item in poolObjects)
            {
                foreach (var obj in item.Value)
                {
                    if (obj != null)
                        Destroy(obj);
                }
                item.Value.Clear();
            }
            poolObjects.Clear();
            foreach (var item in allocatedObjets)
            {
                if (item != null)
                    Destroy(item);
            }
            allocatedObjets.Clear();
            Resources.UnloadUnusedAssets();
        }

        public override void Dispose()
        {
            Clear();
            base.Dispose();
        }
    }

}