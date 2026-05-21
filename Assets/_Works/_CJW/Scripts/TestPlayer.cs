using System;
using System.Linq;
using _Works._CJW.Scripts;
using UnityEngine;

namespace _Works._CJW
{
    public class TestPlayer : MonoBehaviour
    {
        private PlayerInteractManageModule _interactModule;

        private void Awake()
        {
            var compos = GetComponentsInChildren<MonoBehaviour>().First(compo => compo is PlayerInteractManageModule);
            var compo = compos as CameraController;
        }
    }
}