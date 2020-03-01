using UnityEngine;
namespace MiniFramework
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public float Distance = 15;
        [Range(0, 90)]
        public float Angle = 45;
        [Range(0f, 1f)]
        public float lerpSpeed = 0.5f;
        Vector3 targetPos;
        Vector3 targetAngle;
        // Use this for initialization
        void Start()
        {

        }
        private void FixedUpdate()
        {
            if (Target != null)
            {
                targetPos = Target.position - transform.forward * Distance;

                transform.position = Vector3.Slerp(transform.position, targetPos, lerpSpeed);

                targetAngle = new Vector3(Angle, 0, 0);
                transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetAngle, lerpSpeed);

            }
        }
    }
}

