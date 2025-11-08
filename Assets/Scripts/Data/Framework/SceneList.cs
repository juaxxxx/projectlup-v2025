using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList", menuName = "Scriptable Objects/SceneList")]
public class SceneList : ScriptableObject
{
    public List<SceneAsset> scenes = new List<SceneAsset>();
}
