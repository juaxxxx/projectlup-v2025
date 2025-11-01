using UnityEngine;
using TMPro;

public class Result_CharacterDisplay : MonoBehaviour
{
    [SerializeField] private Transform characterParent; // 캐릭터 오브젝트를 생성할 부모 (Empty)
    [SerializeField] private GameObject capsulePrefab;  // 임시 Capsule Prefab
    [SerializeField] private float spacing = 2f;        // 캐릭터 간 간격

    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public int contribution;
    }

    public CharacterData[] characters;

    public void DisplayCharacters()
    {
        // 기존에 있던 Capsule 제거 (다시 표시할 경우 대비)
        foreach (Transform child in characterParent)
            Destroy(child.gameObject);

        // 캐릭터 수만큼 Capsule 생성
        for (int i = 0; i < characters.Length; i++)
        {
            GameObject cap = Instantiate(capsulePrefab, characterParent);
            cap.name = characters[i].name;
            cap.transform.localPosition = new Vector3(i * spacing, 0, 0);
        }
    }
}