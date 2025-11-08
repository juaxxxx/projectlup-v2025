using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList", menuName = "Scriptable Objects/SceneList")]
public class SceneList : ScriptableObject
{
    public List<SceneAsset> scenes = new List<SceneAsset>();
}
