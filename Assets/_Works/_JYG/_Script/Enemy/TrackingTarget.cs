using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Works._JYG._Script.Enemy
{
    [RequireComponent(typeof(AudioSource))]
    public class TrackingTarget : MonoBehaviour
    {
        [SerializeField] private AudioSource findingSource;

        private AbstractEnemy _enemy;

        private bool isFind = false;

        private void Awake()
        {
            _enemy = GetComponentInParent<AbstractEnemy>();
            if(findingSource == null)
                findingSource = GetComponent<AudioSource>();
            
            findingSource.pitch = Random.Range(0.5f, 1f);
        }

        private void Update()
        {
            if (_enemy.IsDead)
            {
                StopAllCoroutines();
                findingSource.Stop();
            }
            if (Mathf.Approximately(_enemy.GetEnemyCaution, 1) && !isFind)
            {
                isFind = true;
                StartCoroutine(SetActiveFalse());
                return;
            }
            if (_enemy.GetEnemyCaution > 0.1f)
            {
                findingSource.volume = _enemy.GetEnemyCaution;
            }
            else
            {
                findingSource.volume = 0f;
            }
        }

        private IEnumerator SetActiveFalse()
        {
            float time = 0;
            float percent = 0;
            while (percent < 1)
            {
                if (_enemy.IsDead) yield break;
                percent = (time) / 1;
                
                findingSource.volume = (1 - percent);
                
                time += Time.deltaTime;
                yield return null;
            }
            findingSource.Stop();
            gameObject.SetActive(false);
        }
    }
}
