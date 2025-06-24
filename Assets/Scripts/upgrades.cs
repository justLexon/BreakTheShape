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
        if (shapeManager.GetCoinCount() >= 50)
        {
            // Spend 50 coins
            shapeManager.SpendCoins(50);

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


    public void RockUpgrade()
    {
        if (shapeManager.GetCoinCount() >= 100)
        {
            // Spend 100 coins
            shapeManager.SpendCoins(100);

            // Upgrade tap damage
            int index = shapeManager.GetCurrentShapeIndex();
            shapeManager.tapDamage += 3;

            Debug.Log("Tap Damage upgraded! Now: " + shapeManager.tapDamage);
        }
        else
        {
            Debug.Log("Not enough coins for upgrade.");
        }
    }


    public void BatUpgrade()
    {
        if (shapeManager.GetCoinCount() >= 300)
        {
            // Spend 300 coins
            shapeManager.SpendCoins(300);

            // Upgrade tap damage
            int index = shapeManager.GetCurrentShapeIndex();
            shapeManager.tapDamage += 5;

            Debug.Log("Tap Damage upgraded! Now: " + shapeManager.tapDamage);
        }
        else
        {
            Debug.Log("Not enough coins for upgrade.");
        }
    }


}
