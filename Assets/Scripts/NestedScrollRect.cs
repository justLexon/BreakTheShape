using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class NestedScrollRectHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    [SerializeField] private ScrollRect parentScrollRect;

    private ScrollRect currentScrollRect;
    private bool routeToParent = false;
    private bool directionChosen = false;

    void Awake()
    {
        currentScrollRect = GetComponent<ScrollRect>();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        currentScrollRect.OnInitializePotentialDrag(eventData);
        parentScrollRect.OnInitializePotentialDrag(eventData);
        directionChosen = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!directionChosen)
        {
            routeToParent = Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x);
            directionChosen = true;
        }

        if (routeToParent)
            parentScrollRect.OnBeginDrag(eventData);
        else
            currentScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (routeToParent)
            parentScrollRect.OnDrag(eventData);
        else
            currentScrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (routeToParent)
            parentScrollRect.OnEndDrag(eventData);
        else
            currentScrollRect.OnEndDrag(eventData);

        routeToParent = false;
        directionChosen = false;
    }

    private bool ShouldRouteToParent(PointerEventData eventData)
    {
        return Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x);
    }
}
