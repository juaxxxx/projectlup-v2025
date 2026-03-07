using UnityEngine;
using UnityEditor;
namespace LUP.PCR
{
    [CustomEditor(typeof(StructureBase), true)]
    public class PCRStructureEditor : Editor
    {
        protected virtual void OnSceneGUI()
        {
            StructureBase structure = (StructureBase)target;

            if (structure.entranceAnchor == null)
            {
                return;
            }

            // Waypoint 핸들 그리기
            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < structure.localWaypoints.Count; i++)
            {
                // 로컬 -> 월드 변환
                Vector3 worldPos = structure.transform.TransformPoint(structure.localWaypoints[i]);

                // 씬 뷰에 이동 핸들(화살표) 표시
                Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

                // 점 위에 "1", "2" 숫자 표시
                Handles.Label(newWorldPos + Vector3.up * 0.5f, $"{i + 1}");

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(structure, "Move Waypoint");
                    // 월드 -> 로컬 변환해서 저장
                    structure.localWaypoints[i] = structure.transform.InverseTransformPoint(newWorldPos);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            StructureBase structure = (StructureBase)target;

            GUILayout.Space(10);
            GUILayout.Label("경로 편집 도구", EditorStyles.boldLabel);

            if (GUILayout.Button("Way Point 추가"))
            {
                Undo.RecordObject(structure, "Add Waypoint");

                // 마지막 점(혹은 입구) 위치에 새 점 추가
                Vector3 lastPos = structure.localWaypoints.Count > 0
                    ? structure.localWaypoints[structure.localWaypoints.Count - 1]
                    : Vector3.zero; // 로컬 0,0,0 (건물 중심)

                structure.localWaypoints.Add(lastPos + new Vector3(1, 0, 0)); // 살짝 옆에 생성
            }

            if (GUILayout.Button("마지막 포인트 삭제"))
            {
                if (structure.localWaypoints.Count > 0)
                {
                    Undo.RecordObject(structure, "Remove Waypoint");
                    structure.localWaypoints.RemoveAt(structure.localWaypoints.Count - 1);
                }
            }
        }
    }
}