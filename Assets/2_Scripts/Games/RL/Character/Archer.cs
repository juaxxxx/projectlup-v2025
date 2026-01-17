using LUP.ES;
using Roguelike.Define;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace LUP.RL
{
    public class Archer : MonoBehaviour, IHideInterface
    {
        [Header("캐릭터 템플릿 데이터 (SO)")]
        [SerializeField] private RLCharacterData characterTemplate;


        [Header("런타임 데이터")]
        private RunTimeData runtimeData; 


        [SerializeField] public LevelDataTable levelTable;
        //public BaseStats stats;

        //[Header("버프 관련")]
        //public List<BuffData> allBuffs;          // 인스펙터에서 등록
        //public GameObject buffSelectionUI;       // 전체 패널 (Canvas 안)
        //public Transform optionParent;           // 버튼들을 넣을 부모 (예: Horizontal Layout)
        //public GameObject optionButtonPrefab;    // 버튼 프리팹 (Text만 있어도 OK)

        public event System.Action OnExpChanged;
        public event System.Action OnArcherDataReady;
        List<BuffData> GetBuffList = new List<BuffData>();

        [Header("UI ")]
        [SerializeField]
        private Hpbar hpbar;
        public GameObject HpbarPrefab;
        private HealthCenter healthCenter;
        [SerializeField] private float hpbaroffsetY = 5;
        public int PendingExp;

        // 외부 접근용 프로퍼티
        public RunTimeData RuntimeData => runtimeData;
        public HealthCenter HealthCenter => healthCenter;

        private PlayerBuff playerBuff;
        public PlayerBuff PlayerBuff => playerBuff;
        void Awake()
        {
            InitializeCharacter();
            healthCenter = new HealthCenter(RuntimeData.currentData.MaxHp);

        }
        private void Start()
        {
            InitaliUI();
            playerBuff = GetComponent<PlayerBuff>();
            playerBuff.init(this);
            playerBuff.ShowBuffSelection();
        }
        public void HideUI()
        {
            hpbar.HpBarDisable();
        }
        public void ShowUI()
        {
            hpbar.HpBarEnable();
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
                //case BuffType.AddAtkHigh:
                //    RuntimeData.currentData.AttackSpeed += 3;
                //    break;
                
            }
            GetBuffList.Add(buff);
        }
        private void OnEnable()
        {
         
            //Enemy.OnEnemyDied += GainExp;
            if(healthCenter != null)
            {
                healthCenter.OnDead += Die;
            }
         

        }
        public void TakeDamage(int damage)
        {
            healthCenter.Damage(damage);

        }
        private void Die()
        {
            Destroy(gameObject, 0.1f);
        }
        private void OnDisable()
        {
            //Enemy.OnEnemyDied -= GainExp;
        }
        //private void GainExp(int exp)
        //{
        //    var data = levelTable.GetLevelData(RuntimeData.level);
        //    RuntimeData.xp += exp;
        //    Debug.Log($"이번 스테이지 누적 EXP : {PendingExp}");
        //    if (RuntimeData.xp >= data.RequiredExp)
        //        LevelUp();
        //    OnExpChanged?.Invoke();
        //}

        public void LevelUp()
        {
            RuntimeData.level++;
            //RuntimeData.xp = 0;
            var levelData = levelTable.GetLevelData(RuntimeData.level);
            if (levelData != null)
            {
                RuntimeData.currentData.Attack = levelData.AttackBouns;
                RuntimeData.currentData.Hp = levelData.HpBouns;
                RuntimeData.currentData.MaxHp = levelData.HpBouns;
            }
            playerBuff.ShowBuffSelection();
            Debug.Log($"레벨{RuntimeData.level}, 체력 :  {RuntimeData.currentData.Hp} ,  공격력  {RuntimeData.currentData.Attack}");
        }
        public void RaiseExpChanged()
        {
            OnExpChanged?.Invoke();
        }

    }
}