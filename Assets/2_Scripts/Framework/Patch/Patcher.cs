using LUP.Define;
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
        private AssetBundleManifest AB_Manifest;

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
                case Define.AssetBundleKind.Video:
                    filename = "videos";
                    break;
                case Define.AssetBundleKind.Audio:
                    filename = "audios";
                    break;
                case Define.AssetBundleKind.Image:
                    filename = "image";
                    break;
                case Define.AssetBundleKind.VFX:
                    filename = "vfx";
                    break;
                case Define.AssetBundleKind.GUI:
                    filename = "gui";
                    break;
                case Define.AssetBundleKind.Model:
                    filename = "models";
                    break;
                case Define.AssetBundleKind.Shader:
                    filename = "shaders";
                    break;
                case Define.AssetBundleKind.Data:
                    filename = "data";
                    break;
                case Define.AssetBundleKind.Manifest:
                    filename = "AssetBundles";
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

                string assetBundleDirectory = Path.Combine(Application.persistentDataPath, "LUP/assetbundles");
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

            yield return DownloadResource(Define.AssetBundleKind.Manifest);
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

            yield return CheckVersions();

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
            if (versionsdata.Videohash != tempversionsdata.Videohash)
            {
                differentlist.Add(Define.AssetBundleKind.Video);
            }
            if (versionsdata.Audiohash != tempversionsdata.Audiohash)
            {
                differentlist.Add(Define.AssetBundleKind.Audio);
            }
            if (versionsdata.Imagehash != tempversionsdata.Imagehash)
            {
                differentlist.Add(Define.AssetBundleKind.Image);
            }
            if (versionsdata.VFXhash != tempversionsdata.VFXhash)
            {
                differentlist.Add(Define.AssetBundleKind.VFX);
            }
            if (versionsdata.GUIhash != tempversionsdata.GUIhash)
            {
                differentlist.Add(Define.AssetBundleKind.GUI);
            }
            if (versionsdata.Modelhash != tempversionsdata.Modelhash)
            {
                differentlist.Add(Define.AssetBundleKind.Model);
            }
            if (versionsdata.Shaderhash != tempversionsdata.Shaderhash)
            {
                differentlist.Add(Define.AssetBundleKind.Shader);
            }
            if (versionsdata.Datahash != tempversionsdata.Datahash)
            {
                differentlist.Add(Define.AssetBundleKind.Data);
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
            ResourceManager.Instance.UnLoadAssetBundles();
            ResourceManager.Instance.LoadAssetBundles();
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

        private IEnumerator CheckVersions()
        {
            
            if (!HasLocalManifest())
            {
                versionsdata.Videohash = "";
                versionsdata.Audiohash = "";
                versionsdata.Imagehash = "";
                versionsdata.VFXhash = "";
                versionsdata.GUIhash = "";
                versionsdata.Modelhash = "";
                versionsdata.Shaderhash = "";
                versionsdata.Datahash = "";
            }
            else
            {
                AB_Manifest = LUP.ResourceManager.Instance.GetAssetBundle(Define.AssetBundleKind.Manifest).LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                versionsdata.Videohash = AB_Manifest.GetAssetBundleHash("videos").ToString();

                versionsdata.Audiohash = AB_Manifest.GetAssetBundleHash("audios").ToString();

                versionsdata.Imagehash = AB_Manifest.GetAssetBundleHash("image").ToString();

                versionsdata.VFXhash = AB_Manifest.GetAssetBundleHash("vfx").ToString();

                versionsdata.GUIhash = AB_Manifest.GetAssetBundleHash("gui").ToString();

                versionsdata.Modelhash = AB_Manifest.GetAssetBundleHash("models").ToString();

                versionsdata.Shaderhash = AB_Manifest.GetAssetBundleHash("shaders").ToString();

                versionsdata.Datahash = AB_Manifest.GetAssetBundleHash("data").ToString();
            }
            

            versionsdata.SaveData();

            yield break;
        }

        private bool HasLocalManifest()
        {
            string path = Path.Combine(
                Application.persistentDataPath,
                "LUP/assetbundles",
                "manifest"
            );

            return File.Exists(path);
        }
    }
}
