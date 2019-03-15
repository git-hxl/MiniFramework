using MiniFramework;
using UnityEngine;

namespace MiniFramework
{
    public class Circle : MonoBehaviour
    {
        public float Radius;
        [Range(0, 360)]
        public int Angle;
        public Color Color = Color.yellow;
        void OnDrawGizmos()
        {
            Gizmos.color = Color;
            int t;
            if (transform.forward.z < 0)
            {
                t = Angle / 2 + 360 - (int)Vector3.Angle(transform.forward, Vector3.right);
            }
            else
            {
                t = Angle / 2 + (int)Vector3.Angle(transform.forward, Vector3.right);
            }
            for (int i = t - Angle; i < t; i++)
            {
                float x1 = Radius * Mathf.Cos(i * 3.14f / 180);
                float z1 = Radius * Mathf.Sin(i * 3.14f / 180);
                Vector3 start = transform.position + new Vector3(x1, 0, z1);
                float x2 = Radius * Mathf.Cos((i + 1) * 3.14f / 180);
                float z2 = Radius * Mathf.Sin((i + 1) * 3.14f / 180);
                Vector3 end = transform.position + new Vector3(x2, 0, z2);
                Gizmos.DrawLine(start, end);
            }
            if (Angle < 360)
            {
                float x1 = Radius * Mathf.Cos((t - Angle) * 3.14f / 180);
                float z1 = Radius * Mathf.Sin((t - Angle) * 3.14f / 180);
                Vector3 start = transform.position + new Vector3(x1, 0, z1);
                float x2 = Radius * Mathf.Cos(t * 3.14f / 180);
                float z2 = Radius * Mathf.Sin(t * 3.14f / 180);
                Vector3 end = transform.position + new Vector3(x2, 0, z2);
                Gizmos.DrawLine(start, transform.position);
                Gizmos.DrawLine(end, transform.position);
            }
        }
    }
}