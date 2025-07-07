using UnityEngine;

public class upgradesOverlay : MonoBehaviour
{
    [Header("Overlay Pages")]
    public GameObject upgrades;
    public GameObject DPSupgrades;
    public GameObject materials;

    [Header("Game Manager")]
    public GameObject gameManager; // Drag the GameManager GameObject here in the Inspector

    private ShapeManager shapeManager;

    private GameObject[] upgradePages;
    private int currentPage = 0;

    void Start()
    {
        shapeManager = gameManager.GetComponent<ShapeManager>();

        // Set up all pages
        upgradePages = new GameObject[] { upgrades, DPSupgrades, materials };

        // Hide all at start
        foreach (GameObject page in upgradePages)
        {
            page.SetActive(false);
        }
    }

    // Toggle overlay visibility
    public void ToggleUpgradesOverlay()
    {
        bool anyActive = false;
        foreach (GameObject page in upgradePages)
        {
            if (page.activeSelf)
            {
                anyActive = true;
                break;
            }
        }

        if (anyActive)
        {
            // Hide all if any are active
            foreach (GameObject page in upgradePages)
            {
                page.SetActive(false);
            }
        }
        else
        {
            // Show the current page only
            ShowPage(currentPage);
        }
    }

    void ShowPage(int pageIndex)
    {
        for (int i = 0; i < upgradePages.Length; i++)
        {
            upgradePages[i].SetActive(i == pageIndex);
        }
    }

    public void NextPage()
    {
        currentPage = (currentPage + 1) % upgradePages.Length;
        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        currentPage = (currentPage - 1 + upgradePages.Length) % upgradePages.Length;
        ShowPage(currentPage);
    }

    // === Upgrade Buttons ===

    public void FistUpgrade()
    {
        if (shapeManager.GetCoinCount() >= 100)
        {
            shapeManager.SpendCoins(100);
            shapeManager.tapDamage += 1;
            Debug.Log("Tap Damage upgraded! Now: " + shapeManager.tapDamage);
        }
        else
        {
            Debug.Log("Not enough coins for Fist upgrade.");
        }
    }

    public void RockUpgrade()
    {
        if (shapeManager.GetCoinCount() >= 500)
        {
            shapeManager.SpendCoins(500);
            shapeManager.tapDamage += 5;
            Debug.Log("Tap Damage upgraded! Now: " + shapeManager.tapDamage);
        }
        else
        {
            Debug.Log("Not enough coins for Rock upgrade.");
        }
    }

    public void BatUpgrade()
    {
        if (shapeManager.GetCoinCount() >= 1000)
        {
            shapeManager.SpendCoins(1000);
            shapeManager.tapDamage += 15;
            Debug.Log("Tap Damage upgraded! Now: " + shapeManager.tapDamage);
        }
        else
        {
            Debug.Log("Not enough coins for Bat upgrade.");
        }
    }
}
