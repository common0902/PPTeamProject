using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class DOTweenCinemachineTest : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource cinemachine;
    private void Start()
    {
        transform.DOMove(new Vector3(0, 0, 0), 2f);
    }
}
