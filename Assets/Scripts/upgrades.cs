using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject upgrades;
    public GameObject gameManager; // Drag the GameManager GameObject here in the Inspector

    private ShapeManager shapeManager;

    public void ToggleUpgradeOverlay()
    {
        bool isActive = upgrades.activeSelf;
        upgrades.SetActive(!isActive);
    }


    void Start()
    {
        // Get the Shape script component from the GameObject GameManager
        shapeManager = gameManager.GetComponent<ShapeManager>();
    }

   public void FistUpgrade()
{
    if (shapeManager.GetCoinCount() >= 500)
    {
        // Spend 500 coins
        shapeManager.SpendCoins(500);

        // Upgrade tap damage
        int index = shapeManager.GetCurrentShapeIndex();
        shapeManager.tapDamage += 1;

        Debug.Log("Tap Damage upgraded! Now: " + shapeManager.tapDamage);
    }
    else
    {
        Debug.Log("Not enough coins for upgrade.");
    }
}


}
