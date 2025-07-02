using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FastScrollRect : ScrollRect
{
    protected override void Start()
    {
        base.Start();
        // Lower the drag threshold to reduce input lag
        this.scrollSensitivity = 20f; // you can tweak this
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        // Optionally, you can do more here
    }
}
