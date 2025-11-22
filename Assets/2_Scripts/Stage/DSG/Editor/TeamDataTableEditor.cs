using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TeamDataTable))]
public class TeamDataTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty teamsProp = serializedObject.FindProperty("teams");
        int maxTeams = TeamDataTable.MaxTeams;

        // 배열 크기를 강제로 고정 (항상 8개 유지)
        if (teamsProp.arraySize != maxTeams)
        {
            while (teamsProp.arraySize < maxTeams)
                teamsProp.InsertArrayElementAtIndex(teamsProp.arraySize);

            while (teamsProp.arraySize > maxTeams)
                teamsProp.DeleteArrayElementAtIndex(teamsProp.arraySize - 1);
        }

        // 제목
        EditorGUILayout.LabelField($"Teams", EditorStyles.boldLabel);

        // 팀 슬롯들 표시
        for (int i = 0; i < maxTeams; i++)
        {
            SerializedProperty teamProp = teamsProp.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(teamProp, new GUIContent($"Team {i + 1}"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}