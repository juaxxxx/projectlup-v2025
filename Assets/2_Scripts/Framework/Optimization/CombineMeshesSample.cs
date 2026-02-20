using UnityEngine;
using System.Collections.Generic;

public class CombineMeshesSample : MonoBehaviour
{
    void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        Dictionary<Material, List<MeshFilter>> dict = new();

        foreach (var r in renderers)
        {
            if (r.transform == transform) continue;

            if (!dict.ContainsKey(r.sharedMaterial))
                dict[r.sharedMaterial] = new List<MeshFilter>();

            dict[r.sharedMaterial].Add(r.GetComponent<MeshFilter>());
        }

        foreach (var pair in dict)
        {
            Material mat = pair.Key;
            List<MeshFilter> filters = pair.Value;

            CombineInstance[] combine = new CombineInstance[filters.Count];

            for (int i = 0; i < filters.Count; i++)
            {
                combine[i].mesh = filters[i].sharedMesh;
                combine[i].transform = filters[i].transform.localToWorldMatrix;
                filters[i].gameObject.SetActive(false);
            }

            GameObject go = new GameObject("Combined_" + mat.name);
            go.transform.SetParent(transform);

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine);

            mf.mesh = mesh;
            mr.sharedMaterial = mat;
        }
    }
}