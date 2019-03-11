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
        public Sprite[] NumberSprites = new Sprite[10];
        [SerializeField]
        private uint number;

        public uint Number
        {
            get
            {
                return number;
            }
            set
            {
                GenerateNumber(value);
            }
        }
        private void Start()
        {

        }
        void GenerateNumber(uint number)
        {
            if (NumberSprites.Length < 10)
            {
                return;
            }
            char[] chars = number.ToString().ToCharArray();
            int curChildCount = transform.childCount;
            if (curChildCount < chars.Length)
            {
                for (int i = curChildCount; i < chars.Length; i++)
                {
                    GameObject obj = new GameObject(i.ToString(), typeof(Image));
                    obj.transform.SetParent(transform,false);
                }
            }
            else if (curChildCount > chars.Length)
            {
                for (int i = chars.Length; i < curChildCount; i++)
                {
                    int lastIndex = transform.childCount - 1;
                    DestroyImmediate(transform.GetChild(lastIndex).gameObject);
                }
            }
            for (int i = 0; i < chars.Length; i++)
            {
                Image image = transform.GetChild(i).GetComponent<Image>();
                image.raycastTarget = false;
                int num = int.Parse(chars[i].ToString());
                image.sprite = NumberSprites[num];
            }
        }
    }

}