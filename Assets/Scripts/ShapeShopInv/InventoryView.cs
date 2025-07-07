using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public ShapePack[] shapePacks;
    public GameObject packRowPrefab;
    public Transform packContainer;
    public InventoryManager inventoryManager;

    void Start()
    {
        foreach (ShapePack pack in shapePacks)
        {
            GameObject rowObj = Instantiate(packRowPrefab, packContainer);
            ShapePackUI rowUI = rowObj.GetComponent<ShapePackUI>();
            rowUI.Setup(pack, inventoryManager);

            foreach (var item in pack.shapes)
            {
                inventoryManager.allShapes.Add(item);
            }
        }
    }
}
