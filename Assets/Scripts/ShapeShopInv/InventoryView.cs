using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public Transform packRowContainer;
    public GameObject packRowPrefab;

    void Start()
    {
        foreach (var pack in InventoryManager.Instance.shapePacks)
        {
            var row = Instantiate(packRowPrefab, packRowContainer);
            var packUI = row.GetComponent<ShapePackUI>();
            packUI.PopulatePack(pack);
        }
    }
}
