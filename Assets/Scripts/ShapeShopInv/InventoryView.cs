using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryView : MonoBehaviour
{
    public static InventoryView Instance;
    public Transform packContainer; // Content container under ScrollView
    public GameObject packUIPrefab; // Prefab for a row (ShapePackUI)
    public ScrollRect verticalScrollRect; // assign in inspector
    public GameObject inventoryOverlayPanel; // Assign the inventory root panel here
    public TMP_Text currentShapeEnabledText;


    private void Awake()
    {
        Instance = this;
    }

    public void PopulateFromShop()
    {
        Populate(ShopManager.Instance.allShapePacks);
    }

    public void Populate(List<ShapePack> allPacks)
    {
        foreach (Transform child in packContainer)
            Destroy(child.gameObject);

        foreach (var pack in allPacks)
        {
            GameObject packGO = Instantiate(packUIPrefab, packContainer);
            ShapePackUI packUI = packGO.GetComponent<ShapePackUI>();

            NestedScrollRectHandler nestedHandler = packGO.GetComponent<NestedScrollRectHandler>();
            if (nestedHandler != null)
            {
                nestedHandler.SetParentScrollRect(verticalScrollRect);
            }

            packUI.Setup(pack);
        }
    }

    // Call this method from your close button OnClick event
    public void TryCloseInventory()
    {
        int enabledCount = InventoryManager.Instance.GetEnabledShapes().Count;

        if (enabledCount != 10)
        {
            Debug.LogWarning($"⚠️ You must enable exactly 10 shapes before closing the inventory. Currently enabled: {enabledCount}");
            // Optional: Show UI warning message here instead of debug log
            return;
        }

        // Close inventory
        inventoryOverlayPanel.SetActive(false);

        // Optional: refresh ShapeManager or game state if needed
        ShapeManager.Instance.RefreshEnabledShapes();
    }

    public void UpdateCurrentShapeEnabledText()
    {
        int enabledCount = InventoryManager.Instance.GetEnabledShapes().Count;
        currentShapeEnabledText.text = $"{enabledCount}/10";
    }

}
