using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniFramework
{
    public enum JoyStickState
    {
        None,
        OnBeginDrag,
        OnDrag,
        OnEndDrag,
    }
    public class Joystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public RectTransform Rocker;//摇杆
        public RectTransform Base;//底座
        public RectTransform Arrow;
        public float MaxDistance;//摇杆最大移动距离
        public Action OnBeginDragHandler;//摇杆开始拖拽事件
        public Action<Vector2> OnDragHandler;//摇杆拖拽中事件
        public Action OnEndDragHandler;//摇杆结束拖拽事件
        public JoyStickState CurState;//摇杆当前状态
        public Vector2 RockerDir { get { return Rocker.localPosition.normalized; } }//摇杆方向
        private Vector2 rockerPos;

        private int pointerID = -1;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (pointerID == -1)
            {
                pointerID = eventData.pointerId;
            }
            else
            {
                return;
            }
            Base.gameObject.SetActive(true);
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.position, eventData.enterEventCamera, out position);
            Base.localPosition = position;
            rockerPos = Rocker.localPosition;
            if (OnBeginDragHandler != null)
            {
                OnBeginDragHandler();
            }
            CurState = JoyStickState.OnBeginDrag;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != pointerID)
            {
                return;
            }
            rockerPos += eventData.delta;
            float distance = Vector2.Distance(rockerPos, Vector2.zero);
            if (distance > MaxDistance)
            {
                rockerPos = (MaxDistance / distance) * rockerPos;
            }
            Rocker.localPosition = rockerPos;
            if (Arrow != null)
            {
                Arrow.localPosition = Rocker.localPosition.normalized * MaxDistance;
                Arrow.up = Rocker.localPosition.normalized;
            }
            if (OnDragHandler != null)
            {
                OnDragHandler(Rocker.localPosition.normalized);
            }
            CurState = JoyStickState.OnDrag;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != pointerID)
            {
                return;
            }
            Rocker.localPosition = Vector3.zero;
            if (OnEndDragHandler != null)
            {
                OnEndDragHandler();
            }
            CurState = JoyStickState.OnEndDrag;
            Base.gameObject.SetActive(false);
            pointerID = -1;
        }
    }
}