using System;
using System.Collections;
using UnityEngine;

namespace _Works._CJW.Scripts.Objects.Box
{
    public class DropItem : MonoBehaviour
    {
        [SerializeField] private float bounceHeight = 0.5f;
        [SerializeField] private float bounceDuration = 0.5f;
        [SerializeField] private float bounceSpeed;
        [SerializeField] private float timer;
        private BoxCollider _collider;

        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            StartCoroutine(fTime());
            StartCoroutine(BouncingCoroutine());
        }
        private IEnumerator fTime()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(timer);
            _collider.enabled = true;
        }
        private IEnumerator BouncingCoroutine()
        {
            float t = 0;
            while (t < bounceDuration)
            {
                float height = Mathf.Cos(Mathf.PI * (t / bounceDuration)) * bounceHeight;
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x, transform.position.y + height, transform.position.z),
                    Time.deltaTime * bounceSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(0,360 * (t / bounceDuration), 0),
                    Time.deltaTime * bounceSpeed);
                t += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(BouncingCoroutine());
        }
    }
}