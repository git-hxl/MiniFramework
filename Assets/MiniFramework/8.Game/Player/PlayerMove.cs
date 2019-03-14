using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        public float Speed;
        public Vector3 MoveDir;
        private Rigidbody mRigidbody;
        private Joystick mJoystick;
        
        void Start()
        {
            mRigidbody = GetComponent<Rigidbody>();
            GameObject obj = UIManager.Instance.GetUI("Joystick");
            if (obj != null)
            {
                mJoystick = obj.GetComponent<Joystick>();
            }
        }
        void Update()
        {
            MoveDir.x = Input.GetAxis("Horizontal");
            MoveDir.z = Input.GetAxis("Vertical");
        }
        void FixedUpdate()
        {
            if (mJoystick != null && mJoystick.CurState == JoyStickState.OnDrag)
            {
                MoveDir = new Vector3(mJoystick.Rocker.localPosition.x, 0, mJoystick.Rocker.localPosition.y);
            }
            if (MoveDir != Vector3.zero)
            {
                mRigidbody.MoveRotation(Quaternion.LookRotation(MoveDir));
                mRigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
            }
        }
    }
}