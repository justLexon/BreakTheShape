using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public Transform packContainer; // Content container under ScrollView
    public GameObject packUIPrefab; // Prefab for a row (ShapePackUI)

    public void Populate(List<ShapePack> allPacks)
    {
        foreach (Transform child in packContainer)
            Destroy(child.gameObject); // Clear old

        foreach (var pack in allPacks)
        {
            GameObject packGO = Instantiate(packUIPrefab, packContainer);
            ShapePackUI packUI = packGO.GetComponent<ShapePackUI>();
            packUI.Setup(pack);
        }
    }
}
