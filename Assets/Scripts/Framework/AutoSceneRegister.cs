using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;

public class AutoSceneRegister
{
    [MenuItem("Tools/Build/Auto Register Scenes")]
    public static void AutoRegisterScenes()
    {
        // "Assets/Scenes" 폴더 기준 (원하면 경로 바꿔도 됨)
        string sceneFolderPath = "Assets/Scenes";

        // 폴더 안의 모든 .unity 파일 찾기
        string[] allScenePaths = Directory.GetFiles(sceneFolderPath, "*.unity", SearchOption.AllDirectories);

        // EditorBuildSettingsScene 리스트로 변환
        var newSceneList = allScenePaths.Select(path => new EditorBuildSettingsScene(path, true)).ToArray();

        // 적용
        EditorBuildSettings.scenes = newSceneList;

        Debug.Log($"✅ {newSceneList.Length}개의 씬이 빌드 설정에 자동 등록되었습니다!");
    }
}
