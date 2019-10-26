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
        void LateUpdate()
        {
            if (Target != null)
            {
                Vector3 targetPos = Target.position - mCamera.forward * Distance;
                mCamera.position = Vector3.Lerp(mCamera.position, targetPos, Time.deltaTime * 5);
                mCamera.eulerAngles = Vector3.Lerp(mCamera.eulerAngles, new Vector3(Angle, 0, 0), Time.deltaTime * 5);
            }
        }
    }
}

