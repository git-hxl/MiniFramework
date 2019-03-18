using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class CanvasFollow : MonoBehaviour
    {
        GameObject m_Player;
        Vector3 m_Offset;
        // Use this for initialization
        void Start()
        {
            m_Player = GameObject.FindGameObjectWithTag("Player");
            if(m_Player!=null){
                m_Offset = transform.position - m_Player.transform.position;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Player != null)
            {
                transform.position = m_Player.transform.position + m_Offset;
            }
        }
    }
}