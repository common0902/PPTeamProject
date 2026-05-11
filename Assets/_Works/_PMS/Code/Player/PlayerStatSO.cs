using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatSO", menuName = "Scriptable Objects/PlayerStatSO")]
public class PlayerStatSO : ScriptableObject
{
    [field: SerializeField] public bool IsGun { get; private set; } = false;
    [field: SerializeField] public float Hp { get; private set; } = 100f;
    [field: SerializeField] public float ViewMapCooldown { get; private set; } = 5.0f;
    [field: SerializeField] public float RunCooldown { get; private set; } = 2.0f;


}
