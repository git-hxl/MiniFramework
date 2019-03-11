using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniFramework
{
    public class JoyStick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public RectTransform Joy;
        public float MaxDistance;
        public float Sensitivity;
        //public Vector3 GetJoyDir { get { return } }
        private Vector2 startInputPos;
        private Vector3 startJoyStickPos;
        private RectTransform joyStick;
        private Vector2 tempPos;
        void Start()
        {
            joyStick = GetComponent<RectTransform>();
            startJoyStickPos = joyStick.position;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                joyStick.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                joyStick.position = startJoyStickPos;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            startInputPos = eventData.position;
            tempPos = Joy.localPosition;
        }
        public void OnDrag(PointerEventData eventData)
        {
            tempPos += eventData.delta;
            if (Vector3.Distance(Vector3.zero, tempPos) > MaxDistance)
            {
                
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Joy.localPosition = Vector3.zero;
        }
    }

}
