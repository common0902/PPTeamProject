using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace _Works._JTH.TimeLine
{
    public class TimeLineManager : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(WaitForStart(GetComponent<PlayableDirector>()));
        }

        private IEnumerator WaitForStart(PlayableDirector director)
        {
            yield return new WaitForSeconds(0.5f);
            director.Play();
        }
    }
}
