using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Overlay Pages")]
    public GameObject IAP;
    public GameObject shopPacks;
    public GameObject accessories;

    [Header("Game Manager")]
    public GameObject gameManager; // Drag the GameManager GameObject here in the Inspector

    private ShapeManager shapeManager;

    private GameObject[] shopPages;
    private int currentPage = 1;

    void Start()
    {
        shapeManager = gameManager.GetComponent<ShapeManager>();

        // Set up all pages
        shopPages = new GameObject[] { IAP, shopPacks, accessories };

        // Hide all at start
        foreach (GameObject page in shopPages)
        {
            page.SetActive(false);
        }
    }

    // Toggle overlay visibility
    public void ToggleUpgradesOverlay()
    {
        bool anyActive = false;
        foreach (GameObject page in shopPages)
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
            foreach (GameObject page in shopPages)
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
        for (int i = 0; i < shopPages.Length; i++)
        {
            shopPages[i].SetActive(i == pageIndex);
        }
    }

    public void NextPage()
    {
        currentPage = (currentPage + 1) % shopPages.Length;
        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        currentPage = (currentPage - 1 + shopPages.Length) % shopPages.Length;
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
