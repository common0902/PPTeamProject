using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Works._CJW.Scripts.Rendering
{
    public static class MeshCombiner
    {
        public static Mesh CombineMesh(
            GameObject targetObj,
            List<FOVRendering> children)
        {
            Mesh mesh = new Mesh();

            List<CombineInstance> combines = new();

            int vertexCount = 0;

            foreach (FOVRendering child in children)
            {
                if (child == null)
                    continue;

                if (child.MeshFilter == null)
                    continue;

                if (child.MeshFilter.sharedMesh == null)
                    continue;

                CombineInstance combine = new CombineInstance
                {
                    mesh = child.MeshFilter.sharedMesh,
                    transform =
                        child.MeshFilter.transform.localToWorldMatrix
                };

                combines.Add(combine);

                vertexCount +=
                    child.MeshFilter.sharedMesh.vertexCount;
            }

            if (vertexCount > 65535)
            {
                mesh.indexFormat = IndexFormat.UInt32;
            }

            mesh.CombineMeshes(combines.ToArray());

            targetObj.SetActive(true);

            return mesh;
        }
    }
}