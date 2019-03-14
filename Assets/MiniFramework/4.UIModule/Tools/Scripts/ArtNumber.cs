using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFramework
{
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class ArtNumber : MonoBehaviour
    {
        public List<Sprite> Sprites;
        
        [SerializeField]
        private string number;
        [SerializeField]
        private float size;
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                Create(value);
            }
        }
        private Dictionary<string, Sprite> spriteDict;
        private void Start()
        {
            Generate();
        }
        public void Generate()
        {
            spriteDict = new Dictionary<string, Sprite>();
            for (int i = 0; i < Sprites.Count; i++)
            {
                spriteDict.Add(Sprites[i].name, Sprites[i]);
            }
        }
        void Create(string value)
        {
            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                
                Transform child;
                if (i > transform.childCount - 1)
                {
                    GameObject obj = new GameObject(i.ToString(), typeof(Image));
                    obj.transform.SetParent(transform, false);
                    child = obj.transform;
                }
                child = transform.GetChild(i);
                Image image = child.GetComponent<Image>();
                image.raycastTarget = false;
                if(spriteDict.ContainsKey(chars[i].ToString())){
                    image.sprite = spriteDict[chars[i].ToString()];
                } 
            }
            for (int i = chars.Length; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }

}