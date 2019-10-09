using UnityEngine;
namespace MiniFramework
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public float Distance;
        public float Angle;
        private Transform mCamera;
        // Use this for initialization
        void Start()
        {
            mCamera = Camera.main.transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Target != null)
            {
                //mCamera.LookAt(Target);
                mCamera.position = Vector3.Lerp(mCamera.position, Target.position - mCamera.forward * Distance, Time.deltaTime*10);
                mCamera.eulerAngles = Vector3.Lerp(mCamera.eulerAngles, new Vector3(Angle, 0, 0), Time.deltaTime*10);
            }
        }
    }
}

