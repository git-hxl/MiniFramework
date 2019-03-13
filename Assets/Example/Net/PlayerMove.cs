using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class PlayerMove : MonoBehaviour
{
    public float Speed;
    private Rigidbody mRigidbody;
    private Joystick mJoystick;
    private Vector3 mMoveDir;
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
		mJoystick = UIManager.Instance.GetUI("Joystick").GetComponent<Joystick>();
    }
    void Update()
    {
        mMoveDir.x = Input.GetAxis("Horizontal");
        mMoveDir.z = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        if (mJoystick != null&&mJoystick.CurState == JoyStickState.OnDrag)
        {
            mMoveDir = new Vector3(mJoystick.Rocker.localPosition.x, 0, mJoystick.Rocker.localPosition.y);
        }
        if (mMoveDir != Vector3.zero)
        {
            mRigidbody.MoveRotation(Quaternion.LookRotation(mMoveDir));
            mRigidbody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
        }
    }
}
