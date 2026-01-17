using DG.Tweening;
using Roguelike.Define;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{

    public class FloatingItemPopupImage : MonoBehaviour
    {

        public Action<int, int> OnPopupDisApear;

        [HideInInspector]
        public int custumItemID;

        //카운트 용 획득 아이템 개수
        [HideInInspector]
        public int itemGainedAmount;

        //실제 추가 될 아이템 개수
        public int totalGainningAmount = 0;

        private FloatinItemUIPoll spanwedPool;

        public Image itemBackGround;
        public Image itemimage;
        public TextMeshProUGUI owningAmountText;
        public TextMeshProUGUI gainedAmountText;

        private float UIWidth = 0;
        private float UIHeight = 0;


        //////////////AnimationProperty////////////
        [SerializeField]
        private float countDownSpeed = 30f;

        [SerializeField] 
        private float waitAfterZero = 1.0f;

        [SerializeField]
        private float waitAfterMovingLeft = 0.7f;

        [SerializeField]
        private float OnChanginAmountDelay = 0.8f;

        [SerializeField] 
        private float moveDuration = 0.4f;

        [SerializeField]
        private float disappearXOffset = 10.0f;
        private float movingYOffset = 25.0f;

        private float waitTimerforAfterZero;
        private float waitTimerforAfterMoveLeft;
        private int displayingAmount;

        [SerializeField]
        public int displayedOwningAmount;

        private Tween moveTween;

        public FloatingImageState uiState = FloatingImageState.Sleep;
        //////////////AnimationProperty////////////



        public RectTransform rect;
        private void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();

            if (rect)
            {
                UIWidth = rect.rect.width;
                UIHeight = rect.rect.height;
            }
                
        }

        public Vector2 InitFloatingItemImage(RLDropItemType itemType, int customID, Sprite itemImage, Int32 owningAmount, Int32 gainedAmount, FloatinItemUIPoll spanwer)
        {
            custumItemID = customID;
            itemGainedAmount = gainedAmount;
            totalGainningAmount += gainedAmount;

            displayedOwningAmount = owningAmount;

            spanwedPool = spanwer;

            //장비일때는 파란색, 소모품일때는 초록색
            if (itemType == RLDropItemType.Commodities)
            {
                itemBackGround.color = Color.green;
            }


            else if (itemType == RLDropItemType.equipment)
            {
                itemBackGround.color = Color.blue;
            }

            itemimage.sprite = itemImage;
            owningAmountText.text = owningAmount.ToString();
            gainedAmountText.text = gainedAmount.ToString();

            uiState = FloatingImageState.MovingLeft;

            return new Vector2(UIWidth, UIHeight);
        }

        public void MoveLeft()
        {
            uiState = FloatingImageState.MovingLeft;

            DOTween.Kill(this);

            Sequence seq = DOTween.Sequence()
                .SetTarget(this);

            seq.Append(
                rect.DOAnchorPosX(
                    rect.anchoredPosition.x - UIWidth,
                    moveDuration
                )
            );

            seq.AppendInterval(waitAfterMovingLeft);

            seq.AppendCallback(() =>
            {
                EnterCountState();
            });

            moveTween = seq;
        }

        private void EnterCountState()
        {
            uiState = FloatingImageState.Counting;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (uiState != FloatingImageState.Counting)
                return;

            if (itemGainedAmount <= 0)
            {
                uiState = FloatingImageState.WaitingDisappear;
                return;
            }

            float delta = Time.deltaTime * 0.1f;
            int decrease = Mathf.CeilToInt(delta);

            itemGainedAmount -= decrease;
            displayedOwningAmount += decrease;

            if (itemGainedAmount < 0)
                itemGainedAmount = 0;

            displayingAmount = itemGainedAmount;
            gainedAmountText.text = displayingAmount.ToString();
            owningAmountText.text = displayedOwningAmount.ToString();
        }

        

        private void LateUpdate()
        {
            if (uiState != FloatingImageState.WaitingDisappear)
                return;

            waitTimerforAfterZero += Time.deltaTime;
            if (waitTimerforAfterZero >= waitAfterZero)
            {
                MoveRight();
            }
        }

        private void MoveRight()
        {
            uiState = FloatingImageState.MovingRight;

            moveTween?.Kill(this);

            moveTween = rect
                .DOAnchorPosX(rect.anchoredPosition.x + UIWidth + disappearXOffset, moveDuration)
                .SetTarget(this)
                .OnComplete(OnMovingRightComplete);
        }

        public void OnGainedAmountChanged(int addedAmount)
        {
            totalGainningAmount += addedAmount;
            gainedAmountText.text = itemGainedAmount.ToString();

            if (uiState == FloatingImageState.MovingLeft)
                return;

            waitTimerforAfterZero = 0;

            DOTween.Kill(this);

            uiState = FloatingImageState.Pause;

            Sequence seq = DOTween.Sequence().SetTarget(this);

            seq.AppendInterval(OnChanginAmountDelay);

            seq.AppendCallback(() =>
            {
                EnterCountState();
            });

            moveTween = seq;
        }


        private void OnMovingRightComplete()
        {
            moveTween.Kill(this);
            spanwedPool.ReturnUI(this);

            ResetProperty();
        }


        private void ResetProperty()
        {
            totalGainningAmount = 0;
        }

    }
}

