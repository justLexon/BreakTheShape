using UnityEngine;
using UnityEngine.UI;

public class ShapeIconUI : MonoBehaviour
{
    public Image shapeImage;
    public GameObject lockOverlay;      // Gray overlay if not owned
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
        {
            Debug.Log("⛔ Shape not owned!");
            return;
        }

        // Attempt to toggle
        if (!isEnabled && InventoryManager.Instance.GetEnabledShapes().Count >= 10)
        {
            Debug.Log("❌ Max 10 shapes can be enabled.");
            return;
        }

        isEnabled = !isEnabled;
        InventoryManager.Instance.SetShapeEnabled(shapeId, isEnabled);
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
