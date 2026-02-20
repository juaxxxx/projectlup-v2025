using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class GPUInstancer : MonoBehaviour
{
    struct Batch
    {
        public Mesh mesh;
        public Material mat;
        public Matrix4x4[] matrices;
        public int count;
    }

    List<Batch> batches = new();

    void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        Dictionary<(Mesh, Material), List<Matrix4x4>> groups = new();

        foreach (var r in renderers)
        {
            MeshFilter mf = r.GetComponent<MeshFilter>();
            if (!mf) continue;

            var key = (mf.sharedMesh, r.sharedMaterial);

            if (!groups.ContainsKey(key))
                groups[key] = new List<Matrix4x4>();

            groups[key].Add(r.transform.localToWorldMatrix);

            // ¿øº» ¼û±è
            r.gameObject.SetActive(false);
        }

        // ¹èÄ¡ »ý¼º (µü ÇÑ¹ø¸¸ °è»ê)
        foreach (var g in groups)
        {
            var list = g.Value;
            int total = list.Count;

            const int batchSize = 1023;
            int offset = 0;

            while (offset < total)
            {
                int count = Mathf.Min(batchSize, total - offset);
                Matrix4x4[] arr = new Matrix4x4[count];

                list.CopyTo(offset, arr, 0, count);

                batches.Add(new Batch
                {
                    mesh = g.Key.Item1,
                    mat = g.Key.Item2,
                    matrices = arr,
                    count = count
                });

                offset += count;
            }
        }
    }

    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += Render;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= Render;
    }

    void Render(ScriptableRenderContext ctx, Camera cam)
    {
        foreach (var b in batches)
        {
            Graphics.DrawMeshInstanced(
                b.mesh,
                0,
                b.mat,
                b.matrices,
                b.count
            );
        }
    }
}