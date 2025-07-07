using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [Header("Overlay Pages")]
    public GameObject inventory;

    void Start()
    {
        if (inventory != null)
            inventory.SetActive(false); // Start inactive
    }

    // Toggle overlay visibility
    public void ToggleInventoryOverlay()
    {
        if (inventory == null) return;

        bool isActive = inventory.activeSelf;
        inventory.SetActive(!isActive); // Toggle state
    }
}
