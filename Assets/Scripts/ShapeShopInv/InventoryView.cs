using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public Transform packContainer; // Content container under ScrollView
    public GameObject packUIPrefab; // Prefab for a row (ShapePackUI)


    public void PopulateFromShop()
    {
        Populate(ShopManager.Instance.allShapePacks);
    }

    public ScrollRect verticalScrollRect; // assign this in inspector (your vertical scroll view)

    public void Populate(List<ShapePack> allPacks)
    {
        foreach (Transform child in packContainer)
            Destroy(child.gameObject);

        foreach (var pack in allPacks)
        {
            GameObject packGO = Instantiate(packUIPrefab, packContainer);
            ShapePackUI packUI = packGO.GetComponent<ShapePackUI>();

            // Set the parentScrollRect reference dynamically here
            NestedScrollRectHandler nestedHandler = packGO.GetComponent<NestedScrollRectHandler>();
            if (nestedHandler != null)
            {
                nestedHandler.SetParentScrollRect(verticalScrollRect);
            }

            packUI.Setup(pack);
        }
    }

}
