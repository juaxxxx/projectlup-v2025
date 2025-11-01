using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UserData.Team))]
public class TeamDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 기본 속성들 찾기
        SerializedProperty charactersProp = property.FindPropertyRelative("characters");

        // Layout 기반으로 그리기 위해 BeginProperty 호출
        EditorGUI.BeginProperty(position, label, property);

        // Foldout (Team 항목 접기/펼치기)
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label, true);
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // characters 배열 표시
            EditorGUILayout.LabelField("Characters");

            // 고정 크기로 강제
            int maxSize = 5;
            charactersProp.arraySize = maxSize;

            for (int i = 0; i < maxSize; i++)
            {
                var element = charactersProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent($"Slot {i + 1}"));
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }
}