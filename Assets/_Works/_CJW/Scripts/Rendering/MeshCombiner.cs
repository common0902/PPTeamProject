using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Works._CJW.Scripts.Rendering
{
    public static class MeshCombiner
    {
            // 오브젝트의 자식 메쉬들을 Combine해서 반환하는 정적 메서드
        public static Mesh CombineMesh(GameObject targetObj, FOVRendering[] children) 
        {
            Mesh mesh = new Mesh();
            CombineInstance[] combine = new CombineInstance[children.Length];

            int vertexCount = 0;
            for (int i = 0; i < children.Length; ++i)
            {
                if(children[i] == null) continue;

                combine[i].mesh = children[i].MeshFilter.sharedMesh;
                combine[i].transform = children[i].MeshFilter.transform.localToWorldMatrix;
                children[i].MeshFilter.gameObject.SetActive(false);
                Debug.Log(children[i].MeshFilter.sharedMesh);
                vertexCount += children[i].MeshFilter.sharedMesh.vertexCount;
            }

            if (vertexCount > 65535)
            {
                mesh.indexFormat = IndexFormat.UInt32;
            }
            
            mesh.CombineMeshes(combine);
            targetObj.SetActive(true);
            return mesh;
        }
    }
}