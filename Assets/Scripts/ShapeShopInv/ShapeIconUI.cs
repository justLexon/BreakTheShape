using UnityEngine;
using UnityEngine.UI;

public class ShapeIconUI : MonoBehaviour
{
    public Image shapeImage;
    public GameObject lockOverlay;      // Shown if not owned
    public GameObject enabledOverlay;   // Green overlay if enabled

    private string shapeId;
    private bool isOwned;
    private bool isEnabled;

    public void Setup(string id, Sprite sprite, bool owned, bool enabled)
    {
        shapeId = id;
        isOwned = owned;
        isEnabled = enabled;

        shapeImage.sprite = sprite;
        UpdateVisuals();
    }

    public void OnClick()
    {
        

        if (!isOwned)
            return;

        bool currentlyEnabled = InventoryManager.Instance.IsShapeEnabled(shapeId);

        if (currentlyEnabled)
        {
            InventoryManager.Instance.SetShapeEnabled(shapeId, false);
            ShapeManager.Instance.RefreshEnabledShapes(); // ✅ Refresh when disabled
        }
        else if (InventoryManager.Instance.GetEnabledShapes().Count < 10)
        {
            InventoryManager.Instance.SetShapeEnabled(shapeId, true);
            ShapeManager.Instance.RefreshEnabledShapes(); // ✅ Refresh when enabled
        }

        UpdateVisuals();
    }




    public void UpdateVisual(bool enabled)
    {
        isEnabled = enabled;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        lockOverlay.SetActive(!isOwned);
        enabledOverlay.SetActive(isOwned && isEnabled);
    }
}
