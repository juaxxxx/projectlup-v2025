using OpenCvSharp.Dnn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace LUP
{
    public class Patcher : MonoBehaviour
    {
        public static Patcher Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("[Patcher] 이미 Patcher 인스턴스가 존재합니다. 중복 오브젝트를 파괴합니다.");
                Destroy(gameObject);
            }
        }

        [Header("CDN Settings")]
        // 서버 URL
        [SerializeField] private string url = "https://project-lup.github.io/projectlup-v2025-cdn/";
        [SerializeField]
        private VersionsData versionsdata;      // 로컬에 있는 버전 데이터
        [SerializeField]
        private VersionsData tempversionsdata;  // CDN 에서 받아온 버전 데이터
        [SerializeField]
        private List<Define.AssetBundleKind> differentlist = new List<Define.AssetBundleKind>();

        [Header("Download Progress")]
        private float currentDownloadProgress = 0f;  // 현재 파일 다운로드 진행률 (0~1)
        private int currentFileIndex = 0;            // 현재 다운로드 중인 파일 인덱스
        private int totalFileCount = 0;              // 전체 다운로드할 파일 수

        // 외부에서 읽을 수 있는 전체 진행률 (0~1)
        public float TotalProgress
        {
            get
            {
                if (totalFileCount == 0) return 0f;
                return (currentFileIndex + currentDownloadProgress) / totalFileCount;
            }
        }

        void Start()
        {
            StartCoroutine(PatchFlow());                         
        }

        void Update()
        {

        }

        private IEnumerator DownloadResource(Define.AssetBundleKind assetbundlekind)
        {
            string filename = "";

            switch (assetbundlekind)
            {
                case Define.AssetBundleKind.AssetBundle1:
                    filename = "staticdatas";
                    break;
                default:
                    Debug.LogWarning($"[Patcher] 정의되지 않은 AssetBundleKind: {assetbundlekind}");
                    yield break;
            }

            string fullurl = url + filename;

            using (UnityWebRequest request = UnityWebRequest.Get(fullurl))
            {
                // yield return request.SendWebRequest();

                // 비동기로 요청 시작
                var operation = request.SendWebRequest();

                // 다운로드 진행률 업데이트
                while (!operation.isDone)
                {
                    currentDownloadProgress = request.downloadProgress;
                    yield return null;
                }

#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
                #else
                if (request.isNetworkError || request.isHttpError)
                #endif
                {
                    Debug.LogError($"[Patcher] 에셋번들 다운로드 실패 ({fullurl}): {request.error}");
                    yield break;
                }

                string assetBundleDirectory = Path.Combine(Application.dataPath, "Resources/AssetBundles");
                if (!Directory.Exists(assetBundleDirectory))
                {
                    Directory.CreateDirectory(assetBundleDirectory);
                }

                string filePath = Path.Combine(assetBundleDirectory, filename);
                File.WriteAllBytes(filePath, request.downloadHandler.data);

                Debug.Log($"[Patcher] 에셋번들 다운로드 완료: {fullurl} → {filePath}");
            }

            // 현재 파일 다운로드 완료
            currentDownloadProgress = 1f;
            yield return null;
        }


        private IEnumerator DownloadResources()
        {
            if (differentlist == null || differentlist.Count == 0)
            {
                Debug.Log("[Patcher] 다운로드할 리소스가 없습니다.");
                yield break;
            }

            // 전체 파일 수 설정
            totalFileCount = differentlist.Count;
            currentFileIndex = 0;

            foreach (Define.AssetBundleKind assetbundlekind in differentlist)
            {
                Debug.Log($"[Patcher] 다운로드 시작: {assetbundlekind}");

                currentDownloadProgress = 0f;
                yield return DownloadResource(assetbundlekind);
                currentFileIndex++;
            }

            Debug.Log("[Patcher] 모든 리소스 다운로드 완료");
        }

        private IEnumerator LoadVersions()
        {
            versionsdata = null;

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

            if (versionsdata == null)
            {
                Debug.LogWarning("[Patcher] 로컬 VersionsData를 찾지 못했습니다.");
            }
            else
            {
                Debug.Log("[Patcher] 로컬 VersionsData 로드 완료");
            }

            yield break;
        }


        private IEnumerator LoadTempVersionsFromCDN()
        {
            string versionsJsonUrl = url + "Versions.json";
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

        private IEnumerator CompareVersions()
        {
            differentlist.Clear();
            // 지금 가진거(versionsdata)랑 서버(tempversionsdata)에 있는 버전 체크
            if (versionsdata.assetbundlehash != tempversionsdata.assetbundlehash)
            {
                differentlist.Add(Define.AssetBundleKind.AssetBundle1);
            }
            yield break;
        }
        private IEnumerator PatchFlow()
        {
            // 1. 로컬 버전 데이터 로드
            yield return LoadVersions();

            // 2. CDN에서 버전 데이터 받아오기
            yield return LoadTempVersionsFromCDN();

            // 둘 중 하나라도 없으면 그냥 종료
            if (versionsdata == null || tempversionsdata == null)
            {
                Debug.Log("[Patcher] 버전 데이터가 세팅되지 않아 패치를 중단합니다.");
                yield break;
            }

            // 3. 버전 비교
            yield return CompareVersions();

            if (differentlist.Count==0)
            {
                Debug.Log("[Patcher] 이미 최신버젼입니다.");
                yield break;
            }

            // 4. 리소스 다운로드
            yield return DownloadResources();

            // 5. 버젼 저장
            yield return SaveVersions();
            Debug.Log("[Patcher] 패치 플로우 완료");
        }

        private IEnumerator SaveVersions()
        {
            versionsdata = tempversionsdata;
            versionsdata.SaveData();
            yield break;
        }

        private void CheckAssets()
        {
            if(ResourceManager.Instance.GetAssetBundleSize() > (int)Define.AssetBundleKind.__MAX)
            {
                //애셋번들들 쭉 확인해야함. for문이랑 enum+딕셔너리 사용예정
            }
        }
    }
}
