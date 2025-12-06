using LUP.DSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OpenCvSharp.ML.DTrees;

namespace LUP.DSG
{
    public class CharacterIconGenerator : MonoBehaviour
    {
        [Header("Preview Setup")]
        [SerializeField] private Camera previewCamera;
        [SerializeField] private Transform characterPivot;
        [SerializeField] private RenderTexture renderTexture;

        public IEnumerator GenerateIconRoutine(DeckStrategyStage stage, int cacheKeyCharacterId, int modelId)
        {
            // 이미 캐시에 있으면 스킵
            if (CharacterIconCache.TryGet(cacheKeyCharacterId, out _))
            {
                Debug.Log($"[CharacterIconGenerator] characterId {cacheKeyCharacterId} 는 이미 캐시에 있음, 스킵");
                yield break;
            }

            if (stage == null)
            {
                Debug.LogError("[CharacterIconGenerator] stage 가 null 입니다.");
                yield break;
            }

            // 프리팹 찾을 때는 modelId 사용
            GameObject prefab = stage.GetCharacterPrefab(modelId);
            if (prefab == null)
            {
                Debug.LogError($"[CharacterIconGenerator] modelId {modelId} 프리팹을 찾지 못했습니다.");
                yield break;
            }

            Debug.Log($"[CharacterIconGenerator] Instantiate prefab {prefab.name} for characterId={cacheKeyCharacterId}, modelId={modelId}");

            var instance = Instantiate(prefab, characterPivot.position, characterPivot.rotation);

            yield return new WaitForEndOfFrame();

            // RenderTexture -> Texture2D
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D tex = new Texture2D(
                renderTexture.width,
                renderTexture.height,
                TextureFormat.RGBA32,
                false
            );
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            RenderTexture.active = currentRT;

            // Texture2D -> Sprite
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );

            CharacterIconCache.Set(cacheKeyCharacterId, sprite);

            Debug.Log($"[CharacterIconGenerator] 아이콘 생성 완료. characterId={cacheKeyCharacterId}, modelId={modelId}");

            Destroy(instance);
        }
    }
}