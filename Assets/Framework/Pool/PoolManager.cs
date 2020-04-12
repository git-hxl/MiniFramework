using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniFramework
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        public List<PoolObject> poolObjects = new List<PoolObject>();
        private Dictionary<string, Stack<GameObject>> objs = new Dictionary<string, Stack<GameObject>>();
        public int CurCount(string name)
        {
            return objs.ContainsKey(name) == true ? objs[name].Count : 0;
        }
        public void Add(UnityEngine.Object obj, int min, int max)
        {
            PoolObject poolObject = new PoolObject();
            poolObject.minNum = min;
            poolObject.maxNum = max;
            poolObject.prefab = obj;
            poolObjects.Add(poolObject);
        }
        void Start()
        {
            foreach (var item in poolObjects)
            {
                Create(item.prefab, item.minNum);
            }
        }
        private void Create(UnityEngine.Object obj, int num)
        {
            if (!objs.ContainsKey(obj.name))
            {
                objs.Add(obj.name, new Stack<GameObject>());
            }
            for (int i = objs[obj.name].Count; i < num; i++)
            {
                if (obj != null)
                {
                    GameObject gameObject = Instantiate(obj, transform) as GameObject;
                    gameObject.name = obj.name;
                    gameObject.SetActive(false);
                    objs[obj.name].Push(gameObject);
                }
            }
        }
        //回收
        public void Recycle(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            if (!objs.ContainsKey(obj.name))
            {
                Debug.LogError("该对象不存在缓存池中！无法回收:" + obj.name);
                Destroy(obj.gameObject);
                return;
            }
            if (objs[obj.name].Contains(obj))
            {
                return;
            }
            int max = 0;
            foreach (var item in poolObjects)
            {
                if (item.prefab.name == obj.name)
                {
                    max = item.maxNum;
                }
            }
            if (objs[obj.name].Count >= max)
            {
                Destroy(obj.gameObject);
                return;
            }
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            objs[obj.name].Push(obj);
        }
        //分配
        public GameObject Allocate(string key)
        {
            if (!objs.ContainsKey(key))
            {
                Debug.LogError("该对象不存在缓存池中！无法分配:" + key);
                return null;
            }
            if (CurCount(key) <= 0)
            {
                UnityEngine.Object obj = null;
                foreach (var item in poolObjects)
                {
                    if (item.prefab.name == key)
                    {
                        obj = item.prefab;
                    }
                }
                Create(obj, 1);
            }
            var result = objs[key].Pop();
            result.SetActive(true);
            result.transform.parent = null;
            SceneManager.MoveGameObjectToScene(result, SceneManager.GetActiveScene());
            return result;
        }
        public GameObject Allocate(int index)
        {
            string key = objs.ElementAt(index).Key;
            return Allocate(key);
        }
    }
    [Serializable]
    public class PoolObject
    {
        public int minNum;
        public int maxNum;
        public UnityEngine.Object prefab;
    }
}