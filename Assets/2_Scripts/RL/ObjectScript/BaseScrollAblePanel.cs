using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.UI.ContentSizeFitter;
using static UnityEngine.UI.ScrollRect;

using Roguelike.Define;
using Roguelike.Util;
using System.Collections;

namespace LUP.RL
{
    public abstract class BaseScrollAblePanel : MonoBehaviour
    {
        [SerializeField]
        protected GameObject displayedPrefab;

        public Vector2Int contentSpacking = new Vector2Int(20, 20);
        public double gourPaddingOffsetModifier = 0.3;

        protected ScrollRect scrollRect;
        protected Transform contentParent;

        protected int displayOffset = 0;

        protected IDisplayable[] displayedData;

        private DisplayableDataType calledDataType = DisplayableDataType.None;

        private LayoutDirection layoutDirection = LayoutDirection.None;

        public Vector2 LayoutConstrain = new Vector2(4, 4);

        protected void Start()
        {
            gameObject.SetActive(false);

            //사전 설정 필수
            scrollRect = GetComponentInChildren<ScrollRect>(true);


            if (scrollRect == null)
            {
                UnityEngine.Debug.LogError("Can't Load ScrollAblePanel !!", this.gameObject);
            }
        }

        public void SetScrollPanelType(MovementType movementType, LayoutDirection layout, TextAnchor contentalin = TextAnchor.MiddleCenter)
        {
            if (scrollRect != null)
            {
                bool canScrollVertic = false;
                bool canScrollHorizon = false;

                layoutDirection = layout;

                contentParent = scrollRect.content;

                RectTransform rect = contentParent.GetComponent<RectTransform>();


                switch (layout)
                {
                    case LayoutDirection.Vertical:
                        canScrollVertic = true;
                        if (!contentParent.gameObject.TryGetComponent<VerticalLayoutGroup>(out var existvertic))
                        {
                            existvertic = contentParent.gameObject.AddComponent<VerticalLayoutGroup>();
                            SetContentLayoutToVertic(existvertic, rect);
                        }
                        break;

                    case LayoutDirection.Horizontal:
                        canScrollHorizon = true;
                        if (!contentParent.gameObject.TryGetComponent<HorizontalLayoutGroup>(out var existhorizon))
                        {
                            existhorizon = contentParent.gameObject.AddComponent<HorizontalLayoutGroup>();
                            SetContentLayoutToHorizon(existhorizon, rect);
                        }

                        break;

                    case LayoutDirection.Grid:
                        canScrollVertic = true;
                        if (!contentParent.gameObject.TryGetComponent<GridLayoutGroup>(out var existGrid))
                        {
                            existGrid = contentParent.gameObject.AddComponent<GridLayoutGroup>();
                            SetContentLayoutToGrid(existGrid, rect, (int)LayoutConstrain.x);
                            existGrid.childAlignment = contentalin;
                        }
                        break;
                }

                scrollRect.movementType = movementType;

                scrollRect.vertical = canScrollVertic;
                scrollRect.horizontal = canScrollHorizon;

                ContentSizeFitter SizeFitter = contentParent.AddComponent<ContentSizeFitter>();

                SizeFitter.verticalFit = canScrollVertic ? FitMode.PreferredSize : FitMode.Unconstrained;
                SizeFitter.horizontalFit = canScrollHorizon ? FitMode.PreferredSize : FitMode.Unconstrained;
            }

            else
            {
                UnityEngine.Debug.LogError("Can't Load ScrollAblePanel !!", this.gameObject);
            }
        }

        public void OpenPanel(IDisplayable[] dataArray, DisplayableDataType dataType, int offset = 0)
        {
            gameObject.SetActive(true);
            displayedData = dataArray;
            calledDataType = dataType;

            displayOffset = offset;

            GenerateContent();

        }

        protected virtual void GenerateContent()
        {
            if (displayedData == null)
            {
                UnityEngine.Debug.LogError("DisplayedData is Null", this.gameObject);
                return;
            }

        }

        private void Update()
        {

        }

        protected void OnItemSelected(int selectedIndex)
        {
            LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

            if (lobbyGameCenter == null)
            {
                UnityEngine.Debug.LogError("LRGameCenter Is Null", this.gameObject);
                return;
            }

            lobbyGameCenter.SetSelectedData(calledDataType, selectedIndex);

            ErasePanel();
        }

        void SetContentLayoutToVertic(VerticalLayoutGroup vertical, RectTransform rect)
        {
            int displayableSpacking = contentSpacking.y;

            vertical.spacing = displayableSpacking;
            vertical.childAlignment = TextAnchor.MiddleLeft;

            vertical.childControlHeight = false;
            vertical.childControlWidth = false;

            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
        }

        void SetContentLayoutToHorizon(HorizontalLayoutGroup horizontal, RectTransform rect)
        {
            int displayableSpacking = contentSpacking.x;

            horizontal.spacing = displayableSpacking;
            horizontal.childAlignment = TextAnchor.MiddleLeft;

            horizontal.childControlHeight = false;
            horizontal.childControlWidth = false;

            int paddingOffset = (int)(displayableSpacking * gourPaddingOffsetModifier);

            horizontal.padding.left = displayableSpacking + paddingOffset;
            horizontal.padding.right = displayableSpacking + paddingOffset;

            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
        }

        void SetContentLayoutToGrid(GridLayoutGroup existGrid, RectTransform rect, int numhorizonEle)
        {
            //프리팹 싸이즈에 맞게 자동 조절
            {

                //Vector2 prefabSize = displayedPrefab.GetComponent<RectTransform>().rect.size;
                //Vector2Int displayableSpacking = contentSpacking;

                //existGrid.cellSize = prefabSize;
                //existGrid.spacing = new Vector2(displayableSpacking.x, displayableSpacking.y);
                //existGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
                //existGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
                //existGrid.childAlignment = TextAnchor.MiddleCenter;
                //existGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            }

            //개수 엔진에서 수동 입력 버젼
            {
                existGrid.spacing = new Vector2(10, 20);
                existGrid.padding = new RectOffset(10, 10, 10, 10);
                existGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
                existGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
                existGrid.childAlignment = TextAnchor.MiddleCenter;
                existGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

                Vector2 spacing = existGrid.spacing;
                float prefabWidth = (rect.rect.size.x / numhorizonEle) - 2 * spacing.x;



                existGrid.constraintCount = numhorizonEle;
                existGrid.cellSize = new Vector2(prefabWidth, prefabWidth);
            }


            //만약 수동 조작이 필요한 상황을 위해 남겨둠 -> setconstraintCount prefab 크기에 따라 자동 조절되게 수정
            //existGrid.constraintCount = numhorizonEle;
        }

        protected void RefreshPanel()
        {
            //scrollRect.normalizedPosition = Vector3.zero;
            scrollRect.horizontalNormalizedPosition = 0;
        }


        protected void EraseContents()
        {
            if (contentParent == null)
                return;

            foreach (Transform preBtn in contentParent)
            {
                Destroy(preBtn.gameObject);
            }
        }

        protected void setconstraintCount()
        {
            RefreshPanel();

            Transform viewportTrans = contentParent.parent;
            RectTransform viewportRect = viewportTrans.GetComponent<RectTransform>();

            Vector2 prefabSize = displayedPrefab.GetComponent<RectTransform>().rect.size;
            GridLayoutGroup existGrid = contentParent.gameObject.GetComponent<GridLayoutGroup>();
            Vector2Int displayableSpacking = contentSpacking;

            float resultItemViewportSize = viewportRect.rect.width;

            float countResult = (resultItemViewportSize / (prefabSize.x + displayableSpacking.x));
            int numHorizenConstraintCount = (int)countResult;

            if (countResult - numHorizenConstraintCount >= 0.7)
            {
                numHorizenConstraintCount = numHorizenConstraintCount + 1;
            }

            existGrid.constraintCount = numHorizenConstraintCount;
        }

        protected void ErasePanel()
        {
            calledDataType = DisplayableDataType.None;
            EraseContents();
            contentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            gameObject.SetActive(false);
        }
    }
}


