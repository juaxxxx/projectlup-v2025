using LUP;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class StageBuildEditor
{
    private const string INIT_SCENE_NAME = "Init"; // Init 씬 이름 고정

    [MenuItem("Tools/Register Scenes")]
    public static void RebuildBuildSettings()
    {
        // 현재 작업중인 씬 저장 여부 확인 (수정된 경우 물어봄)
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        // 1) 프로젝트에서 Init 씬 경로 검색
        string[] sceneGUIDs = AssetDatabase.FindAssets(INIT_SCENE_NAME + " t:Scene");
        if (sceneGUIDs.Length == 0)
        {
            Debug.LogError($"❌ '{INIT_SCENE_NAME}' 씬을 찾을 수 없음.");
            return;
        }

        string initScenePath = AssetDatabase.GUIDToAssetPath(sceneGUIDs[0]);

        // 2) Init 씬 열기
        EditorSceneManager.OpenScene(initScenePath);

        // 3) StageManager 찾기
        StageManager manager = Object.FindAnyObjectByType<StageManager>();
        if (manager == null)
        {
            Debug.LogError("❌ Init 씬에서 StageManager를 찾을 수 없음.");
            return;
        }

        // 4) Build Settings 씬 목록 구성
        List<EditorBuildSettingsScene> newBuildScenes = new List<EditorBuildSettingsScene>();
        List<SceneList> stageLists = new List<SceneList>();
        stageLists.Add(manager.FW_StageList);
        stageLists.Add(manager.RL_StageList);
        stageLists.Add(manager.ST_StageList);
        stageLists.Add(manager.ES_StageList);
        stageLists.Add(manager.PCR_StageList);
        stageLists.Add(manager.DSG_StageList);
        foreach (var listSO in stageLists)
        {
            if (listSO == null) continue;

            foreach (var sceneAsset in listSO.scenes)
            {
                if (sceneAsset == null) continue;

                string path = AssetDatabase.GetAssetPath(sceneAsset);
                newBuildScenes.Add(new EditorBuildSettingsScene(path, true));
            }
        }

        // 5) Build Settings 갱신
        EditorBuildSettings.scenes = newBuildScenes.ToArray();

        Debug.Log($"✅ Build Settings 등록 완료! 총 {newBuildScenes.Count}개 씬 적용됨");
    }
}
