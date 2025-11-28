using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LUP.RL
{
    public class PannelInputController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private PannelController controller;

        private Vector2 startPos;
        private Vector2 currentPos;
        private Vector2 endPos;

        public float touchTrheshold = 20;
        public float scrollSpeed = 1.3f;

        [HideInInspector]
        public Scrollbar targetVerticScrollbar;

        private void Start()
        {
            controller = FindFirstObjectByType<PannelController>();

            if (controller == null)
            {
                UnityEngine.Debug.LogError("Cant' Find PannelController");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //startPos = eventData.position;

            //controller.touchStartPos = startPos;

            //controller.TouchStart();
        }

        public void OnDrag(PointerEventData eventData)
        {

            float delta = eventData.delta.y;

            currentPos = eventData.position;

            controller.touchCurrentPos = currentPos;

            //controller.OnDrawing();

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            WorldEventBtnScrollPannel foundSubPanel = null;

            for (int i = 0; i < results.Count; i++)
            {
                WorldEventBtnScrollPannel subScrollPanel = results[i].gameObject.GetComponentInChildren<WorldEventBtnScrollPannel>();

                if (subScrollPanel != null)
                {
                    foundSubPanel = subScrollPanel;
                    break;
                }
            }

            if (foundSubPanel != null)
            {

                foundSubPanel.MoveScroll((delta / controller.pannelSize.y) * scrollSpeed * 2);
            }
            else
            {
                if (targetVerticScrollbar)
                    targetVerticScrollbar.value -= (delta / controller.pannelSize.y) * scrollSpeed;

                controller.DisableWorldEventSubPanel();
            }



        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            startPos = eventData.position;
            controller.touchStartPos = startPos;

            controller.TouchStart();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            endPos = eventData.position;
            controller.touchEndPos = endPos;

            controller.TouchEnd();

            if (JudgeTouch())
            {
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject == gameObject)
                        continue;
                    if ((results[i].gameObject.GetComponent<Button>() != null) || (results[i].gameObject.GetComponent<CircleButton>() != null))
                    {
                        ExecuteEvents.ExecuteHierarchy(
                        results[i].gameObject,
                        eventData,
                        ExecuteEvents.pointerClickHandler
                        );

                    }


                }
            }
        }

        bool JudgeTouch()
        {
            float distance = Vector2.Distance(startPos, endPos);

            if (distance < touchTrheshold)
            {
                return true;
            }

            return false;
        }
    }
}


