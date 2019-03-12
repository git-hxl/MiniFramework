using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniFramework
{
    public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public RectTransform Rocker;//摇杆
        public RectTransform Base;//底座
        public float MaxDistance;//摇杆最大移动距离
        public Action OnDragStart;
        public Action<Vector2> OnDragging;
        public Action OnDragEnd;
        public Vector2 RockerDir { get { return Rocker.localPosition.normalized; } }
        private Vector2 rockerPos;
        private Vector2 basePos;
        void Start()
        {
            basePos = Base.localPosition;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            rockerPos = Rocker.localPosition;
        }
        public void OnDrag(PointerEventData eventData)
        {
            rockerPos += eventData.delta;
            float distance = Vector2.Distance(rockerPos, Vector2.zero);
            if (distance > MaxDistance)
            {
                rockerPos = (MaxDistance / distance) * rockerPos;
            }
            Rocker.localPosition = rockerPos;
            if(OnDragging!=null){
                OnDragging(Rocker.localPosition.normalized);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Rocker.localPosition = Vector3.zero;
            if(OnDragEnd!=null){
                OnDragEnd();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Base, Input.mousePosition, eventData.enterEventCamera, out position);
            Base.localPosition = position + basePos;
            Base.gameObject.SetActive(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Base.localPosition = basePos;
            Base.gameObject.SetActive(false);
        }
    }

}
