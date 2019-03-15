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
            mJoystick.OnEndDragHandler += () =>
            {
                MoveDir = Vector3.zero;
            };
        }
        void Update()
        {
            // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            // {
            //     MoveDir.x = Input.GetAxis("Horizontal");
            //     MoveDir.z = Input.GetAxis("Vertical");
            // }
            // if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            //     MoveDir = Vector3.zero;
        }
        void FixedUpdate()
        {
            if (mJoystick != null && mJoystick.CurState == JoyStickState.OnDrag)
            {
                MoveDir = new Vector3(mJoystick.Rocker.localPosition.x, 0, mJoystick.Rocker.localPosition.y);
            }
            if (MoveDir != Vector3.zero)
            {
                SetLookDir(MoveDir);
                MoveToDir();
            }
        }

        public void SetLookDir(Vector3 dir)
        {
            mRigidbody.MoveRotation(Quaternion.LookRotation(dir));
        }
        public void MoveToDir()
        {
            mRigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
        }
    }
}