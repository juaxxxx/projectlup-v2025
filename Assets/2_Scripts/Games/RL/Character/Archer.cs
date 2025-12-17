using LUP.ES;
using Roguelike.Define;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace LUP.RL
{
    public class Archer : MonoBehaviour
    {
        [Header("캐릭터 템플릿 데이터 (SO)")]
        [SerializeField] private RLCharacterData characterTemplate;


        [Header("런타임 데이터")]
        private RunTimeData runtimeData; 


        [SerializeField] public LevelDataTable levelTable;
        //public BaseStats stats;

        [Header("버프 관련")]
        public List<BuffData> allBuffs;          // 인스펙터에서 등록
        public GameObject buffSelectionUI;       // 전체 패널 (Canvas 안)
        public Transform optionParent;           // 버튼들을 넣을 부모 (예: Horizontal Layout)
        public GameObject optionButtonPrefab;    // 버튼 프리팹 (Text만 있어도 OK)

        public event System.Action OnExpChanged;
        public event System.Action OnArcherDataReady;
        List<BuffData> randomBuffs = new List<BuffData>();
        List<BuffData> GetBuffList = new List<BuffData>();

        [Header("UI ")]
        [SerializeField]
        private Hpbar hpbar;
        public GameObject HpbarPrefab;
        private HealthCenter healthCenter;
        [SerializeField] private float hpbaroffsetY = 5;
        // 외부 접근용 프로퍼티
        public RunTimeData RuntimeData => runtimeData;
        public HealthCenter HealthCenter => healthCenter;


        void Awake()
        {
           
            InitializeCharacter();
            healthCenter = new HealthCenter(RuntimeData.currentData.MaxHp);

        }
        private void Start()
        {
            InitaliUI();
        }
        private void InitializeCharacter()
        {
            if(characterTemplate == null)
            {
                Debug.Log("null template data");
            }
            runtimeData = new RunTimeData(characterTemplate);
            OnArcherDataReady?.Invoke();
            Debug.Log($"[Archer] 캐릭터 초기화 완료 - 체력: {runtimeData.currentData.MaxHp}, 공격력: {runtimeData.currentData.Attack}");
        }
        private void InitaliUI()
        {
            GameObject barObj = Instantiate(HpbarPrefab, transform.position + Vector3.up * hpbaroffsetY, Quaternion.identity);
            hpbar = barObj.GetComponent<Hpbar>();
            hpbar.Init(this);
        }

     
        //버프 뽑기
        void ShowBuffSelection()
        {
            buffSelectionUI.SetActive(true);

            //기존 버튼 제거
            foreach (Transform child in optionParent)
                Destroy(child.gameObject);


            randomBuffs.Clear(); // 리스트 비우기
                                 //버프3개뽑기     
            while (randomBuffs.Count < 3)
            {
                BuffData candidate = allBuffs[UnityEngine.Random.Range(0, allBuffs.Count)];
                if (!randomBuffs.Contains(candidate))
                {
                    randomBuffs.Add(candidate);
                }
            }

            foreach (BuffData buff in randomBuffs)
            {
                //프리팹 복제
                GameObject btnObj = Instantiate(optionButtonPrefab, optionParent);
                //인덱스 번호 매기기

                //  복사본으로  생성
                BuffData copy = ScriptableObject.CreateInstance<BuffData>();
                copy.buffName = buff.buffName;
                copy.SetDisplayableImage(buff.GetDisplayableImage());

                OptionButtonUI btnUI = btnObj.GetComponent<OptionButtonUI>();
                btnUI.SetData(copy, this);


            }
            Time.timeScale = 0;
        }
        public void ApplyBuff(BuffData buff)
        {
            if (buff == null) Debug.Log("null");
            switch (buff.type)
            {
                case BuffType.AddMaxHp:
                    RuntimeData.currentData.Attack += 5;
                    break;

                case BuffType.AddAtkHigh:
                    RuntimeData.currentData.Hp += 30;
                    break;

                case BuffType.AddSpeed:
                    RuntimeData.currentData.speed += 1;
                    break;
            }
            Debug.Log($"버프 적용됨: {buff.name} | HP: {RuntimeData.currentData.Hp}, 공격력: {RuntimeData.currentData.Attack}, 속도: {RuntimeData.currentData.speed}");
            GetBuffList.Add(buff);

            buffSelectionUI.SetActive(false);
            Time.timeScale = 1f;
        }
        public List<BuffData> GetActiveBufflist()
        {
            return GetBuffList;
        }
        private void OnEnable()
        {
         
            Enemy.OnEnemyDied += GainExp;
            if(healthCenter != null)
            {
                Debug.Log("구독");
                healthCenter.OnDead += Die;
            }
         

        }
        public void TakeDamage(int damage)
        {
            healthCenter.Damage(damage);

        }
        private void Die()
        {
            Debug.Log("플레이어 사망");
            Destroy(gameObject, 0.1f);
        }
        private void OnDisable()
        {
            Enemy.OnEnemyDied -= GainExp;
        }
        private void GainExp(int exp)
        {
            var data = levelTable.GetLevelData(RuntimeData.level);
            RuntimeData.xp += exp;
            if (RuntimeData.xp >= data.RequiredExp)
                LevelUp();
            OnExpChanged?.Invoke();
            Debug.Log($"플레이어가 {exp} 경험치 획득! 현재 총 {RuntimeData.xp}");
        }
        private void LevelUp()
        {
            RuntimeData.level++;
            RuntimeData.xp = 0;
            var levelData = levelTable.GetLevelData(RuntimeData.level);
            if (levelData != null)
            {
                RuntimeData.currentData.Attack = levelData.AttackBouns;
                RuntimeData.currentData.Hp = levelData.HpBouns;
                RuntimeData.currentData.MaxHp = levelData.HpBouns;
            }
            ShowBuffSelection();
            Debug.Log($"레벨{RuntimeData.level}, 체력 :  {RuntimeData.currentData.Hp} ,  공격력  {RuntimeData.currentData.Attack}");
        }
        
    }
}