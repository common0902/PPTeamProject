using UnityEngine.AI;

namespace _Works._JYG._Script.Enemy.PatrolSystem
{
    public interface IAISystem
    {
        NavMeshAgent Navmesh { get; }
        bool IsReturnRoute { get; }
        void SetEnemyRoute();
        void StartMove();
        void StopMove();
        EnemyRoute GetCurrentRoute();
        EnemyRoute PrevEnemyRoute { get; }
        public bool IsArrived();
    }
}