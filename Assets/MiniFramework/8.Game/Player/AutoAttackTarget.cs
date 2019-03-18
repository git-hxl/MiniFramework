using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class AutoAttackTarget : MonoBehaviour
    {
        public float AttackDistance;
        public float MaxSearchDistance;
        public string TargetTag;
        public GameObject CurTarget;
        public bool IsFollow;
        private PlayerMove playerMove;
        // Use this for initialization
        void Start()
        {
            playerMove = GetComponent<PlayerMove>();
        }
        void Update()
        {
            if (CurTarget != null)
            {
                if (Vector3.Distance(transform.position, CurTarget.transform.position) > AttackDistance)
                {
                    //CancelTarget();
                    //移动到可攻击范围内
                    Vector3 targetDir = (CurTarget.transform.position - transform.position).normalized;
                    playerMove.SetLookDir(targetDir);
                }
                else
                {
                    Vector3 targetDir = (CurTarget.transform.position - transform.position).normalized;
                    playerMove.SetLookDir(targetDir);
                    playerMove.MoveDir = targetDir;
                }
            }
        }
        /// <summary>
        /// 选择目标
        /// </summary>
        [ContextMenu("SelectTarget")]
        public void SelectTarget()
        {

            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, MaxSearchDistance, Vector3.zero, out hitInfo,
                                   0, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                CancelTarget();
                hitInfo.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                CurTarget = hitInfo.transform.gameObject;
                return;
            }
            // GameObject[] objs = GameObject.FindGameObjectsWithTag(TargetTag);
            // foreach (var item in objs)
            // {
            //     if (Vector3.Distance(item.transform.position, transform.position) < MaxSearchDistance)
            //     {

            //     }
            // }
        }
        /// <summary>
        /// 取消选择目标
        /// </summary>
        public void CancelTarget()
        {
            if (CurTarget == null)
            {
                return;
            }
            CurTarget.GetComponent<MeshRenderer>().material.color = Color.white;
            CurTarget = null;
        }
    }
}