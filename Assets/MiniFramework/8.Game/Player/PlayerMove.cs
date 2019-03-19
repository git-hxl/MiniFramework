using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public float Speed;
    public Vector3 MoveDir;
    public Vector3 MovePos;
    public bool IsGrounded;
    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_Capsule;
    private Joystick m_Joystick;
    private Vector3 m_GroundContactNormal;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        GameObject obj = UIManager.Instance.GetUI("Joystick");
        if (obj != null)
        {
            m_Joystick = obj.GetComponent<Joystick>();
        }
    }
    void FixedUpdate()
    {
        GroundCheck();
        if (m_Joystick != null && m_Joystick.CurState == JoyStickState.OnDrag)//虚拟摇杆输入
        {
            MoveDir.x = m_Joystick.Rocker.localPosition.x;
            MoveDir.z = m_Joystick.Rocker.localPosition.y;
            SetLookDir(MoveDir);
            MoveToDir();
        }
        else if (PlatformUtil.IsEditor())//编译器模式可采用键盘输入
        {
            MoveDir.x = Input.GetAxis("Horizontal");
            MoveDir.z = Input.GetAxis("Vertical");
            if (MoveDir != Vector3.zero)
            {
                SetLookDir(MoveDir);
                MoveToDir();
            }
        }
        if (MovePos != Vector3.zero)//当存在目标位置
        {
            MovePos.y = transform.position.y;
            Vector3 dir = (MovePos - transform.position).normalized;
            SetLookDir(dir);
            MoveToDir();
            if (Vector3.Distance(transform.position, MovePos) < 1)
            {
                MovePos = Vector3.zero;
            }
        }
    }
    /// <summary>
    /// 设置朝向
    /// </summary>
    /// <param name="dir"></param>
    public void SetLookDir(Vector3 dir)
    {
        m_Rigidbody.MoveRotation(Quaternion.LookRotation(dir));
    }
    /// <summary>
    /// 移动
    /// </summary>
    public void MoveToDir()
    {
        Vector3 moveDir = Vector3.ProjectOnPlane(transform.forward, m_GroundContactNormal);
        m_Rigidbody.MovePosition(transform.position + moveDir * Speed * Time.deltaTime);
    }
    /// <summary>
    /// 地板检测
    /// </summary>
    private void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius - 0.01f, Vector3.down, out hitInfo,
                               (m_Capsule.height / 2f - m_Capsule.radius) + 0.02f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
    }
    /// <summary>
    /// 下落到斜坡时调整方向
    /// </summary>
    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius, Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                m_Rigidbody.velocity = Vector3.ProjectOnPlane(m_Rigidbody.velocity, hitInfo.normal);
            }
        }
    }
}