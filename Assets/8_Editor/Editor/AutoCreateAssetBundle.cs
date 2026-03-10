using UnityEditor;
using UnityEngine;
using System.IO;

public static class AutoCreateAssetBundle
{
    [MenuItem("Tools/AssetBundles/AutoAssignAssetBundle")]
    private static void AssignFromFolder()
    {
        var obj = Selection.activeObject;
        if (obj == null)
        {
            Debug.LogWarning("폴더를 선택하세요.");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(obj);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning("폴더만 선택 가능합니다.");
            return;
        }

        // 폴더명 → 번들명
        string bundleName = Path.GetFileName(folderPath).ToLower();

        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 폴더 자체는 스킵
            if (AssetDatabase.IsValidFolder(path))
                continue;
            
            var importer = AssetImporter.GetAtPath(path);
            if (importer == null)
                continue;

            // Scene 또는 cs 파일이면 AssetBundle 제거
            if (path.EndsWith(".unity") || path.EndsWith(".cs"))
            {
                importer.assetBundleName = "";
                continue;
            }

            importer.assetBundleName = bundleName;
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
        Debug.Log($"AssetBundle 지정 완료: {bundleName}");
    }
}
