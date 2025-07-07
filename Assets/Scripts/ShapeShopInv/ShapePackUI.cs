using UnityEngine;

public class ShapePackUI : MonoBehaviour
{
    public Transform iconParent;
    public GameObject shapeIconPrefab;
    private InventoryManager inventoryManager;

    public void Setup(ShapePack pack, InventoryManager manager)
    {
        inventoryManager = manager;

        foreach (ShapeItem item in pack.shapes)
        {
            GameObject iconObj = Instantiate(shapeIconPrefab, iconParent);
            ShapeIconUI iconUI = iconObj.GetComponent<ShapeIconUI>();
            iconUI.Setup(item, inventoryManager);
        }
    }
}
