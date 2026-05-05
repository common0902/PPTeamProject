using _Works._JYG._Script.Enemy.CombatSystem;
using UnityEngine;

public class TestPPPP : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        Debug.Log($"Damage : {damage}, Dir : {hitDirection}");
    }
}
