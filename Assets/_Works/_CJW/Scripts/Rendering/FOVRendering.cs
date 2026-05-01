using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Works._CJW.Scripts.Rendering
{
    public class FOVRendering : MonoBehaviour
    {
        // 나중에 모듈화 해서 ModuleOwner의 위치와 회전에 맞출예정
        [SerializeField] private float maxResolution = 100f;
        [SerializeField] private float angle = 50f;
        private List<FOVInfo> _fovInfos;
        private Coroutine _coroutine;
        public MeshFilter MeshFilter { get; private set; }
        public UnityEvent OnDrawFov;
        private Mesh _mesh;

        private void Awake()
        {
            _fovInfos = new List<FOVInfo>();
            MeshFilter = GetComponent<MeshFilter>();
            _mesh = new Mesh();
            MeshFilter.mesh = _mesh;
        }

        private void Update()
        {
            if(Keyboard.current.tabKey.wasPressedThisFrame)
                DrawFov();
        }

        [ContextMenu("DrawFoV")]
        public void DrawFov()
        {
            FovCheck();
            
            int vertexCount = _fovInfos.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];
            vertices[0] = Vector3.zero;
            
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(_fovInfos[i].HitPoint);
                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }
            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
            
            // OnDrawFov?.Invoke();
        }

        private void FovCheck()
        {
            RaycastHit hitTemp;
            int stepCount = Mathf.RoundToInt(angle * maxResolution);
            float stepAngleSize = angle / stepCount;
            for (int i = 0; i <= stepCount; i++)
            {
                if(Physics.Raycast(transform.position, new Vector3(Mathf.Sin((transform.eulerAngles.y - angle / 2 + stepAngleSize * i) * Mathf.Deg2Rad),
                    0, Mathf.Cos((transform.eulerAngles.y - angle / 2 + stepAngleSize * i) * Mathf.Deg2Rad)), out hitTemp, 10))
                {
                    _fovInfos.Add(new FOVInfo()
                    {
                        Hit = true,
                        HitPoint = hitTemp.point,
                        Distance = hitTemp.distance,
                        HitAngle = transform.eulerAngles.y - angle / 2 + stepAngleSize * i
                    });
                }
                else
                {
                    _fovInfos.Add(new FOVInfo()
                    {
                        Hit = false,
                        HitPoint = transform.position + new Vector3(Mathf.Sin((transform.eulerAngles.y - angle / 2 + stepAngleSize * i) * Mathf.Deg2Rad),
                            0, Mathf.Cos((transform.eulerAngles.y - angle / 2 + stepAngleSize * i) * Mathf.Deg2Rad)) * 10,
                        Distance = maxResolution,
                        HitAngle = transform.eulerAngles.y - angle / 2 + stepAngleSize * i
                    });
                }
            }
        }
    }

    public struct FOVInfo
    {
        public bool Hit;
        public Vector3 HitPoint;
        public float Distance;
        public float HitAngle;
    }
}
