using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class IconBootstrapper : MonoBehaviour
    {
        public static event Action OnAllIconsGenerated;

        [SerializeField] private CharacterIconGenerator iconGenerator;

        

        private void OnEnable()
        {
            StageInitializeInvoker.OnDSGStageInitialize += OnStageInitialize;
        }

        private void OnDisable()
        {
            StageInitializeInvoker.OnDSGStageInitialize -= OnStageInitialize;
        }

        private void OnStageInitialize(DeckStrategyStage stage)
        {
            StartCoroutine(GenerateAllIcons(stage));
        }

        private IEnumerator GenerateAllIcons(DeckStrategyStage stage)
        {
            if (iconGenerator == null)
            {
                Debug.LogError("[IconBootstrapper] iconGenerator 가 비어있습니다.");
                yield break;
            }
            if (stage == null)
            {
                Debug.LogError("[IconBootstrapper] stage 가 null 입니다.");
                yield break;
            }

            var runtime = stage.RuntimeData as DeckStrategyRuntimeData;
            if (runtime == null)
            {
                Debug.LogError($"[IconBootstrapper] RuntimeData 타입이 DeckStrategyRuntimeData 가 아님: {stage.RuntimeData?.GetType().Name}");
                yield break;
            }

            if (runtime.OwnedCharacterList == null || runtime.OwnedCharacterList.Count == 0)
            {
                Debug.LogWarning("[IconBootstrapper] OwnedCharacterList 가 비어 있습니다.");
                yield break;
            }

            Debug.Log($"[IconBootstrapper] OwnedCharacterList Count = {runtime.OwnedCharacterList.Count}");

            foreach (var owned in runtime.OwnedCharacterList)
            {
                int characterId = owned.characterID;   // 캐시에 쓸 키
                int modelId = owned.characterModelID;       // 프리팹 찾을 모델 ID (실제 필드 이름으로 수정!)

                Debug.Log($"[IconBootstrapper] Generate icon. characterId={characterId}, modelId={modelId}");
                yield return iconGenerator.GenerateIconRoutine(stage, characterId, modelId);
            }
            Debug.Log("[IconBootstrapper] 모든 아이콘 생성 완료");
            OnAllIconsGenerated?.Invoke();
        }
    }
}