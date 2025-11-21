using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;   // ★ 추가

namespace LUP
{
    public class Patcher : MonoBehaviour
    {
        [Header("CDN Settings")]
        // 서버에서 Versions.json 받는 URL
        [SerializeField] private string versionsJsonUrl;
        [SerializeField]
        private VersionsData versionsdata;      // 로컬에 있는 버전 데이터
        [SerializeField]
        private VersionsData tempversionsdata;  // CDN 에서 받아온 버전 데이터
        [SerializeField]
        private List<Define.AssetBundleKind> differentlist = new List<Define.AssetBundleKind>();


        void Start()
        {
            LoadVersions();                         // 로컬 데이터 로드
            StartCoroutine(LoadTempVersionsFromCDN()); // CDN 데이터 로드
        }

        void Update()
        {

        }

        void CheckVersion()
        {
            // 지금 가진거(versionsdata)랑 서버(tempversionsdata)에 있는 버전 체크
            // ex)
            // if (versionsdata.assetbundle1hash != tempversionsdata.assetbundle1hash) { ... }
        }

        void DownloadResources()
        {
            // 필요한 리소스만 다운로드 받기
        }

        void LoadVersions()
        {
            List<BaseRuntimeData> runtimeDatas = LUP.DataManager.Instance.GetRuntimeData(Define.StageKind.Main, 1);
            if (runtimeDatas != null && runtimeDatas.Count > 0)
            {
                foreach (var runtimeData in runtimeDatas)
                {
                    if (runtimeData is VersionsData versionData)
                    {
                        versionsdata = versionData;
                        break;
                    }
                }
            }
        }

        private IEnumerator LoadTempVersionsFromCDN()
        {
            if (string.IsNullOrEmpty(versionsJsonUrl))
            {
                Debug.LogError("[Patcher] versionsJsonUrl이 비어있습니다.");
                yield break;
            }

            using (UnityWebRequest www = UnityWebRequest.Get(versionsJsonUrl))
            {
                yield return www.SendWebRequest();

                #if UNITY_2020_1_OR_NEWER
                if (www.result != UnityWebRequest.Result.Success)
                #else
                if (www.isNetworkError || www.isHttpError)
                #endif
                {
                    Debug.LogError($"[Patcher] CDN Versions.json 다운로드 실패: {www.error}");
                    yield break;
                }

                string json = www.downloadHandler.text;
                // Debug.Log($"[Patcher] CDN Versions.json 수신:\n{json}");

                VersionsData data = null;
                try
                {
                    data = JsonUtility.FromJson<VersionsData>(json);
                }
                catch (System.SystemException e)
                {
                    Debug.LogError($"[Patcher] JSON 파싱 실패: {e.Message}");
                    yield break;
                }

                if (data == null)
                {
                    Debug.LogError("[Patcher] JSON 파싱 결과가 null입니다.");
                    yield break;
                }

                tempversionsdata = data;
                //Debug.Log("[Patcher] tempversionsdata 세팅 완료");
            }
        }

        void CompareVersions()
        {

        }
    }
}
