using System;
using _Script.ScriptableObject.Event;
using _Works._CJW.Scripts.Events;
using _Works._JYG._Script.Enemy;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Sabotage.Functions
{
    [RequireComponent(typeof(BoxCollider))]
    public class Gas : MonoBehaviour
    {
        [SerializeField] private Vector3 detectSize;
        [SerializeField] private EventChannelSO cameraEvent;
        [SerializeField] private float lifeTime;
        private BoxCollider _collider;
        private ParticleSystem _particle;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>() ;
            _particle = GetComponentInChildren<ParticleSystem>();
            _collider.size = detectSize;
            _particle.shape.scale.Scale(detectSize * 3);;
            cameraEvent.AddListener<TopViewEvent>(topView =>
            {
                if(topView.IsTopView)
                    Destroy(gameObject, lifeTime);
            });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.deepSkyBlue;
            Gizmos.DrawCube(transform.position, detectSize);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AbstractEnemy>(out var enemy))
            {
                // enemy.
            }
                
        }
    }
}