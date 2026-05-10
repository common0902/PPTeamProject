using UnityEngine;

namespace _Works._JYG._Script.UI
{
    public class LookAtCamera : MonoBehaviour
    { 
        private Camera _mainCam;
        public Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam;
            }
        }

        private void LateUpdate()
        {
            //transform.LookAt(transform.position + MainCam.transform.rotation * Vector3.forward
            //    , MainCam.transform.rotation * Vector3.up);
            transform.up = MainCam.transform.up;
            transform.forward = MainCam.transform.forward;
        }
    }
}
