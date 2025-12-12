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
                yield break;
            }
            if (runtime.OwnedCharacterList != null)
            {
                foreach (var owned in runtime.OwnedCharacterList)
                {
                    int characterId = owned.characterID;
                    int modelId = owned.characterModelID;

                    yield return iconGenerator.GenerateIconRoutine(stage, characterId, modelId);
                }
            }
            var modelIdSet = new HashSet<int>();

            // OwnedCharacterList의 modelId
            if (runtime.OwnedCharacterList != null)
            {
                foreach (var owned in runtime.OwnedCharacterList)
                {
                    modelIdSet.Add(owned.characterModelID);
                }
            }
            if (runtime.Teams != null)
            {
                foreach (var team in runtime.Teams)
                {
                    if (team == null || team.characters == null) continue;

                    foreach (var ch in team.characters)
                    {
                        if (ch == null) continue;
                        modelIdSet.Add(ch.characterModelID);
                    }
                }
            }
            if (stage.characterModelDataTable != null && stage.characterModelDataTable.characterModelDataList != null)
            {
                foreach (var modelData in stage.characterModelDataTable.characterModelDataList)
                {
                    if (modelData == null) continue;

                    int modelId = modelData.ID;

                    if (CharacterIconCache.TryGetByModelId(modelId, out _))
                        continue;

                    yield return iconGenerator.GenerateIconByModelRoutine(stage, modelId);
                }
            }
            IconBootstrapper.OnAllIconsGenerated?.Invoke();
        }
    }
}