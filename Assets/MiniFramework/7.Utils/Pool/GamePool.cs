using System;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class GamePool : MonoSingleton<GamePool>
    {
        private Dictionary<string, Stack<GameObject>> objs = new Dictionary<string, Stack<GameObject>>();
        private Dictionary<string, uint> maxCacheNum = new Dictionary<string, uint>();
        protected override void OnSingletonInit() { }

        public int CurCount(string name)
        {
            return objs.ContainsKey(name) == true ? objs[name].Count : 0;
        }
        public void Init(GameObject prefab, uint maxCount, uint minCount)
        {
            if (prefab == null || prefab.GetComponent<GamePoolable>() == null)
            {
                return;
            }
            if (!objs.ContainsKey(prefab.name))
            {
                objs.Add(prefab.name, new Stack<GameObject>());
                maxCacheNum.Add(prefab.name, maxCount);
            }
            uint initCount = Math.Min(maxCount, minCount);
            if (CurCount(prefab.name) >= initCount)
            {
                initCount = maxCount;
            }
            for (int i = CurCount(prefab.name); i < initCount; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = prefab.name;
                Recycle(obj);
            }
        }
        public GameObject Allocate(string name)
        {
            var result = CurCount(name) == 0 ? null : objs[name].Pop();
            if (result != null)
            {
                result.GetComponent<GamePoolable>().IsRecycled = false;
                result.SetActive(true);
            }
            else if(objs[name].Count>0)
            {
                 return Allocate(name);
            }
            return result;
        }
        public void Recycle(string name)
        {
            GamePoolable[] objs = FindObjectsOfType<GamePoolable>();
            foreach (var item in objs)
            {
                Recycle(item.gameObject);
            }
        }
        public bool Recycle(GameObject obj)
        {
            if (obj == null || obj.GetComponent<GamePoolable>() == null)
            {
                return false;
            }
            if (obj.GetComponent<GamePoolable>().IsRecycled)
            {
                return false;
            }
            if (objs[obj.name].Count >= maxCacheNum[obj.name])
            {
                return false;
            }
            obj.GetComponent<GamePoolable>().IsRecycled = true;
            obj.GetComponent<GamePoolable>().OnRecycled.Invoke();
            obj.SetActive(false);
            objs[obj.name].Push(obj);
            return true;
        }
    }
}