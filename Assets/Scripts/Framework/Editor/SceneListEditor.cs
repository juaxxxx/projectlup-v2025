#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneList))]
public class SceneListEditor : Editor
{
    private SerializedProperty _sceneNamesProp;
    private List<SceneAsset> _sceneAssets = new List<SceneAsset>();

    private void OnEnable()
    {
        _sceneNamesProp = serializedObject.FindProperty("sceneNames");

        // sceneNames → SceneAsset으로 역변환 (있으면)
        _sceneAssets.Clear();
        for (int i = 0; i < _sceneNamesProp.arraySize; i++)
        {
            var name = _sceneNamesProp.GetArrayElementAtIndex(i).stringValue;
            var guids = AssetDatabase.FindAssets($"{name} t:Scene");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                _sceneAssets.Add(sceneAsset);
            }
            else
            {
                _sceneAssets.Add(null);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Scene List", EditorStyles.boldLabel);

        // SceneAsset 리스트 그리기
        int removeIndex = -1;
        for (int i = 0; i < _sceneAssets.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _sceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(_sceneAssets[i], typeof(SceneAsset), false);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                removeIndex = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (removeIndex >= 0)
        {
            _sceneAssets.RemoveAt(removeIndex);
        }

        if (GUILayout.Button("리스트 추가"))
        {
            _sceneAssets.Add(null);
        }

        EditorGUILayout.Space();

        // 변환 버튼
        if (GUILayout.Button("적용하기"))
        {
            ApplyToRuntime();
        }

        // 디버그용으로 현재 sceneNames 보여주기
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("씬 이름", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(_sceneNamesProp, includeChildren: true);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }

    private void ApplyToRuntime()
    {
        _sceneNamesProp.ClearArray();

        for (int i = 0; i < _sceneAssets.Count; i++)
        {
            var sceneAsset = _sceneAssets[i];
            if (sceneAsset == null)
                continue;

            var path = AssetDatabase.GetAssetPath(sceneAsset);

            var sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            int index = _sceneNamesProp.arraySize;
            _sceneNamesProp.InsertArrayElementAtIndex(index);
            _sceneNamesProp.GetArrayElementAtIndex(index).stringValue = sceneName;
        }

        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("SceneList: SceneAsset → SceneName 변환 완료");
    }
}
#endif
