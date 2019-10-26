using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniFramework
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        public List<PoolGroup> Groups = new List<PoolGroup>();
        private Dictionary<string, Stack<GameObject>> objs = new Dictionary<string, Stack<GameObject>>();
        public int CurCount(string name)
        {
            return objs.ContainsKey(name) == true ? objs[name].Count : 0;
        }
        public void Add(UnityEngine.Object obj, int min, int max)
        {
            PoolGroup poolGroup = new PoolGroup();
            poolGroup.MinNum = min;
            poolGroup.MaxNum = max;
            poolGroup.Objects.Add(obj);
            Groups.Add(poolGroup);
        }
        public void AddGroup(UnityEngine.Object[] objs, int min, int max)
        {
            PoolGroup poolGroup = new PoolGroup();
            poolGroup.MinNum = min;
            poolGroup.MaxNum = max;
            poolGroup.Objects = objs.ToList();
            Groups.Add(poolGroup);
        }
        protected override void Init()
        {
            foreach (var item in Groups)
            {
                for (int i = 0; i < item.Objects.Count; i++)
                {
                    Create(item.Objects[i], item.MinNum);
                }
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
            foreach (var item in Groups)
            {
                for (int i = 0; i < item.Objects.Count; i++)
                {
                    if (item.Objects[i].name == obj.name)
                    {
                        max = item.MaxNum;
                    }
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
                Debug.LogError("该对象不存在缓存池中！无法分配:"+key);
                return null;
            }
            if (CurCount(key) <= 0)
            {
                UnityEngine.Object obj = null;
                foreach (var item in Groups)
                {
                    for (int i = 0; i < item.Objects.Count; i++)
                    {
                        if (item.Objects[i].name == key)
                        {
                            obj = item.Objects[i];
                        }
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
    public class PoolGroup
    {
        public int MinNum;
        public int MaxNum;
        public List<UnityEngine.Object> Objects = new List<UnityEngine.Object>();
    }
}