using UnityEngine;
namespace MiniFramework
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public float Distance = 15;
        public float Angle = 45;
        public float LerpSpeed = 20;
        Vector3 targetPos;
        // Use this for initialization
        void Start()
        {

        }
        private void LateUpdate()
        {
            if (Target != null)
            {
                targetPos = Target.position - transform.forward * Distance;

                transform.position =   Vector3.MoveTowards(transform.position, targetPos, LerpSpeed * Time.deltaTime);

                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(Angle, 0, 0), LerpSpeed * Time.deltaTime);
            }
        }
    }
}

