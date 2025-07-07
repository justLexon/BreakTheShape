using UnityEngine;
using UnityEngine.UI;

public class ShapeIconUI : MonoBehaviour
{
    public Image iconImage;
    public GameObject lockOverlay;
    private ShapeItem shapeItem;
    private InventoryManager inventoryManager;

    public void Setup(ShapeItem item, InventoryManager manager)
    {
        shapeItem = item;
        inventoryManager = manager;
        iconImage.sprite = shapeItem.icon;
        UpdateVisual();
    }

    public void OnClick()
    {
        if (!shapeItem.isUnlocked) return;

        if (shapeItem.isEnabled)
        {
            inventoryManager.DisableShape(shapeItem);
        }
        else
        {
            inventoryManager.EnableShape(shapeItem);
        }

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        lockOverlay.SetActive(!shapeItem.isUnlocked);
        iconImage.color = shapeItem.isEnabled ? Color.green : Color.white;
    }
}
