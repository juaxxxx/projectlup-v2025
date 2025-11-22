using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class test : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("ScrollRect to forward events to")]
    public ScrollRect targetScrollRect;

    private PointerEventData cachedEventData;

    public void OnPointerDown(PointerEventData eventData)
    {
        cachedEventData = eventData;
        ForwardEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cachedEventData = eventData;
        ForwardEvent(eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        cachedEventData = eventData;
        ForwardEvent(eventData, ExecuteEvents.dragHandler);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cachedEventData = eventData;
        ForwardEvent(eventData, ExecuteEvents.endDragHandler);
    }

    // ----------------- Helper -----------------
    private void ForwardEvent(PointerEventData eventData, ExecuteEvents.EventFunction<IBeginDragHandler> func)
    {
        if (targetScrollRect != null)
            ExecuteEvents.ExecuteHierarchy(targetScrollRect.gameObject, eventData, func);
    }

    private void ForwardEvent(PointerEventData eventData, ExecuteEvents.EventFunction<IDragHandler> func)
    {
        if (targetScrollRect != null)
            ExecuteEvents.ExecuteHierarchy(targetScrollRect.gameObject, eventData, func);
    }

    private void ForwardEvent(PointerEventData eventData, ExecuteEvents.EventFunction<IEndDragHandler> func)
    {
        if (targetScrollRect != null)
            ExecuteEvents.ExecuteHierarchy(targetScrollRect.gameObject, eventData, func);
    }

    private void ForwardEvent(PointerEventData eventData, ExecuteEvents.EventFunction<IPointerDownHandler> func)
    {
        if (targetScrollRect != null)
            ExecuteEvents.ExecuteHierarchy(targetScrollRect.gameObject, eventData, func);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
