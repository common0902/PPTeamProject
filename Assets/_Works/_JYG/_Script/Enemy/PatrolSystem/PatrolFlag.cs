using UnityEngine;

namespace _Works._JYG._Script.Enemy.PatrolSystem
{
    public class PatrolFlag : MonoBehaviour
    {
        public Transform GetFlagTransform() => transform;
        public Vector3 GetFlagPosition() => transform.position;
    }
}