using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LUP
{
    [Serializable]
    public class StageTransition
    {
        public Define.StageKind start;
        public Define.StageKind end;
    }

    public class StageManager : Singleton<StageManager>
    {
        [Header("스테이지들 설정하는 부분")]
        public SceneList FW_StageList;
        public SceneList RL_StageList;
        public SceneList ST_StageList;
        public SceneList ES_StageList;
        public SceneList PCR_StageList;
        public SceneList DSG_StageList;

        [Header("Fade Settings")]
        private CanvasGroup fadeCanvas;
        [SerializeField] private float fadeDuration = 1f;

        [Header("초기에 오픈될 스테이지 지정")]
        [SerializeField] private Define.StageKind startStageKind;

        [Header("건들지 마세요!!!")]
        [ReadOnly, SerializeField] private Define.StageKind currentStageKind = Define.StageKind.Unknown;

        private BaseStage currentStageInstance= null;
        private bool isTransitioning = false;

        // Transition 검증용 2차원 리스트
        private List<List<Define.StageKind>> transitionTable = new List<List<Define.StageKind>>();

        // StageKind → Scene 이름 매핑
        private Dictionary<Define.StageKind, SceneList> sceneNameMap = new Dictionary<Define.StageKind, SceneList>();

        public override void Awake()
        {
            base.Awake();

            InitializeTransitionTable();
            InitializeFadeCanvas();
            InitializeSceneMap();
            if (currentStageInstance == null)
            {
                LoadStage(startStageKind);
            }    
        }

        private void InitializeFadeCanvas()
        {
            if (!fadeCanvas)
            {
                GameObject fadeObj = GameObject.Find("FadeCanvas");
                if (!fadeObj)
                {
                    fadeObj = new GameObject("FadeCanvas");

                    Canvas canvas = fadeObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.sortingOrder = 999; // 최상단에 렌더링

                    // CanvasScaler 추가
                    UnityEngine.UI.CanvasScaler scaler = fadeObj.AddComponent<UnityEngine.UI.CanvasScaler>();
                    scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    scaler.referenceResolution = new Vector2(1920, 1080);

                    // GraphicRaycaster 추가
                    fadeObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                    fadeCanvas = fadeObj.AddComponent<CanvasGroup>();

                    GameObject fadeImage = new GameObject("FadeImage");
                    fadeImage.layer = LayerMask.NameToLayer("UI"); // UI Layer 설정
                    fadeImage.transform.SetParent(fadeObj.transform, false);

                    UnityEngine.UI.Image image = fadeImage.AddComponent<UnityEngine.UI.Image>();
                    image.color = Color.black;
                    image.raycastTarget = false; // Raycast 불필요

                    // RectTransform 설정 (전체 화면)
                    RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.sizeDelta = Vector2.zero;
                    rectTransform.anchoredPosition = Vector2.zero;

                    // DontDestroyOnLoad 설정
                    DontDestroyOnLoad(fadeObj);

                    Debug.Log("FadeCanvas created and set to DontDestroyOnLoad");
                }
                else
                {
                    fadeCanvas = fadeObj.GetComponent<CanvasGroup>();
                    Debug.Log("FadeCanvas found in scene");
                }
            }

            // 초기 상태: 투명하게 설정 (게임 시작 시 검은 화면이 보이지 않도록)
            if (fadeCanvas)
            {
                fadeCanvas.alpha = 0f;
                fadeCanvas.blocksRaycasts = false;
            }
        }

        // Transition 테이블 초기화
        private void InitializeTransitionTable()
        {
            List<Define.StageKind> Transition = new List<Define.StageKind>();

            // Unknown
            SetTransition(Transition, Define.StageKind.Unknown);
            SetTransition(Transition, Define.StageKind.Debug);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.RL);
            SetTransition(Transition, Define.StageKind.ST);
            SetTransition(Transition, Define.StageKind.DSG);
            SetTransition(Transition, Define.StageKind.ES);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();


            // Debug
            SetTransition(Transition, Define.StageKind.Debug);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.RL);
            SetTransition(Transition, Define.StageKind.ST);
            SetTransition(Transition, Define.StageKind.DSG);
            SetTransition(Transition, Define.StageKind.ES);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();

            // Main
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.RL);
            SetTransition(Transition, Define.StageKind.ST);
            SetTransition(Transition, Define.StageKind.DSG);
            SetTransition(Transition, Define.StageKind.ES);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();

            // Intro
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.Intro);

            AddTransitionToList(Transition);
            Transition.Clear();

            //Roguelike
            SetTransition(Transition, Define.StageKind.RL);
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();

            // Shooting
            SetTransition(Transition, Define.StageKind.ST);
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();

            // ExtractionShooter
            SetTransition(Transition, Define.StageKind.ES);
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();

            // Production
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.PCR);
            SetTransition(Transition, Define.StageKind.RL);
            SetTransition(Transition, Define.StageKind.ST);
            SetTransition(Transition, Define.StageKind.DSG);
            SetTransition(Transition, Define.StageKind.ES);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();

            // DeckStrategy
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.PCR);
            SetTransition(Transition, Define.StageKind.DSG);

            AddTransitionToList(Transition);
            Transition.Clear();

            // Tutorial
            SetTransition(Transition, Define.StageKind.Debug);
            SetTransition(Transition, Define.StageKind.Intro);
            SetTransition(Transition, Define.StageKind.Main);
            SetTransition(Transition, Define.StageKind.RL);
            SetTransition(Transition, Define.StageKind.ST);
            SetTransition(Transition, Define.StageKind.DSG);
            SetTransition(Transition, Define.StageKind.ES);
            SetTransition(Transition, Define.StageKind.PCR);

            AddTransitionToList(Transition);
            Transition.Clear();
        }

        private void SetTransition(List<Define.StageKind> from, Define.StageKind to)
        {
            from.Add(to);
        }

        private void AddTransitionToList(List<Define.StageKind> from)
        {
            List<Define.StageKind> list = new List<Define.StageKind>(from);
            transitionTable.Add(list);
        }

        // Stage 전환 
        public void LoadStage(Define.StageKind targetStageKind, int sceneindex = -1)
        {
            if (isTransitioning)
            {
                Debug.LogWarning("Already transitioning!");
                return;
            }

            // 1. Transition 검증
            if (!IsValidTransition(currentStageKind, targetStageKind))
            {
                Debug.LogError($"Invalid transition: {currentStageKind} → {targetStageKind}");
                return;
            }

            // 2. 전환 시작
            StartCoroutine(TransitionCoroutine(targetStageKind, sceneindex));
        }

        // Transition 검사
        private bool IsValidTransition(Define.StageKind from, Define.StageKind to)
        {
            return transitionTable[(int)from].Contains(to);
        }

        /// Stage 전환 Coroutine
        private IEnumerator TransitionCoroutine(Define.StageKind targetStageKind, int sceneindex= -1)
        {
            isTransitioning = true;

            // Stage Exit 처리
            yield return StartCoroutine(OnStageExit());

            string sceneName;
            if (sceneindex == -1)
            {
                sceneName = sceneNameMap.ContainsKey(targetStageKind)
                ? sceneNameMap[targetStageKind].scenes[0].name.ToString()
                : targetStageKind.ToString();
            }
            else
            {
                sceneName = sceneNameMap[targetStageKind].scenes[sceneindex].name.ToString();
            }
            Debug.Log("SceneName:" + sceneName);
            // 4. Scene 로드
            

            // 씬매니저에 씬이 존재하는지 확인 - 빌드 세팅
            if (SceneManager.GetSceneByName(sceneName).IsValid() == false &&
                SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
            {
                Debug.LogError($"Scene '{sceneName}' not found in Build Settings! Add it to File → Build Settings → Scenes In Build");
                isTransitioning = false;
                yield break;
            }

            UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            if (asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene '{sceneName}'");
                isTransitioning = false;
                yield break;
            }

            while (!asyncLoad.isDone)
            {
                // 로딩 진행도 표시 가능
                float progress = asyncLoad.progress;
                yield return new WaitForSeconds(0.1f);
            }

            currentStageInstance = FindFirstObjectByType<BaseStage>();
            while (currentStageInstance == null)
            {
                currentStageInstance = FindFirstObjectByType<BaseStage>();
                Debug.Log("Find BaseStage");
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log(currentStageInstance);
            yield return StartCoroutine(OnStageEnter());

            currentStageKind = targetStageKind;
            isTransitioning = false;

            Debug.Log("TransitionCoroutine : " + currentStageKind);
        }

        private IEnumerator FadeOut()
        {
            if (!fadeCanvas)
            {
                Debug.LogError("FadeCanvas is null! This should not happen.");
                yield break;
            }

            Debug.Log($"FadeOut Start - FadeCanvas: {fadeCanvas.name}, Alpha: {fadeCanvas.alpha}, Active: {fadeCanvas.gameObject.activeSelf}");
            fadeCanvas.blocksRaycasts = true;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvas.alpha = 1f;
            Debug.Log($"FadeOut End - Alpha: {fadeCanvas.alpha}");
        }

        private IEnumerator FadeIn()
        {
            if (!fadeCanvas)
            {
                Debug.LogError("FadeCanvas is null! This should not happen.");
                yield break;
            }

            Debug.Log($"FadeIn Start - Alpha: {fadeCanvas.alpha}");
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvas.alpha = 0f;
            fadeCanvas.blocksRaycasts = false;
            Debug.Log($"FadeIn End - Alpha: {fadeCanvas.alpha}, Active: {fadeCanvas.gameObject.activeSelf}");
        }

        private IEnumerator OnStageEnter()
        {
            if (currentStageInstance)
            {
                Debug.Log("OnStageEnter");
                yield return StartCoroutine(currentStageInstance.OnStageEnter());
            }
            yield return StartCoroutine(FadeIn());
        }

        private IEnumerator OnStageExit()
        {
            if (currentStageInstance)
            {
                Debug.Log("OnStageExit");
                yield return StartCoroutine(currentStageInstance.OnStageExit());
            }
            yield return StartCoroutine(FadeOut());
        }

        public BaseStage GetCurrentStage()
        {
            return currentStageInstance;
        }

        private void InitializeSceneMap()
        {
            sceneNameMap.Add(Define.StageKind.Debug, FW_StageList);
            sceneNameMap.Add(Define.StageKind.Intro, FW_StageList);
            sceneNameMap.Add(Define.StageKind.Main, FW_StageList);
            sceneNameMap.Add(Define.StageKind.RL, RL_StageList);
            sceneNameMap.Add(Define.StageKind.ST, ST_StageList);
            sceneNameMap.Add(Define.StageKind.DSG, DSG_StageList);
            sceneNameMap.Add(Define.StageKind.ES, ES_StageList);
            sceneNameMap.Add(Define.StageKind.PCR, PCR_StageList);
            sceneNameMap.Add(Define.StageKind.Unknown, FW_StageList);
        }
    }
}