using UnityEngine;

namespace MiniFramework
{
    public static class RangeCheck
    {
        public static bool CircleCheck(Vector3 origin, Vector3 target, float radius)
        {
            if (target.y < origin.y)
            {
                return false;
            }
            else
            {
                target.y = origin.y;
                return Vector3.Distance(origin, target) <= radius ? true : false;
            }

        }

        public static bool FanCheck(Transform origin, Vector3 target, float angle, float radius)
        {
            if (target.y < origin.position.y)
            {
                return false;
            }
            else
            {
                target.y = origin.position.y;
                Vector3 dirTarget = (target - origin.position).normalized;
                float angleTarget = Vector3.Angle(dirTarget, origin.forward);
				
				Vector3 posRight= Quaternion.AngleAxis(angle/2,Vector3.up)*origin.forward*radius+origin.position;
				Vector3 posLeft = Quaternion.AngleAxis(-angle/2,Vector3.up)*origin.forward*radius+origin.position;

				Debug.DrawLine(origin.position,posRight,Color.red);
				Debug.DrawLine(origin.position,posLeft,Color.red);

				Debug.DrawLine(posLeft,origin.forward*radius+origin.position,Color.red);
				Debug.DrawLine(posRight,origin.forward*radius+origin.position,Color.red);

				if(angleTarget <= angle / 2 &&Vector3.Distance(target,origin.position)<=radius)
				{
					return true;
				}
				else
				{
					return false;
				}
            }
        }
        public static bool SphereCheck(Vector3 origin, Vector3 target, float radius)
        {
            return Vector3.Distance(origin, target) <= radius ? true : false;
        }
    }
}