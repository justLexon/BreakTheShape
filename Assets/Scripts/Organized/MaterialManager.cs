using UnityEngine;

[System.Serializable]
public class MaterialData
{
    public string materialName;
    public Sprite[] levelSprites;   // Visual cycling
    public int currentLevel = 0;    // Stats scaling only
    public int baseCost = 50;       // Base cost per tier

    public MaterialData()
    {
        baseCost = 50;
        currentLevel = 0;
    }
}

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance { get; private set; }

    [Header("Materials")]
    public MaterialData[] allMaterials;

    [Header("References")]
    public UIManagerS uiManager;

    private int currentMaterialIndex = 0;
    private int currentVisualSpriteIndex = 0;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentMaterialIndex = 0;
    }

    // ----- MATERIAL GETTERS -----

    public void SetCurrentMaterial(int index)
    {
        if (index >= 0 && index < allMaterials.Length)
        {
            currentMaterialIndex = index;
            currentVisualSpriteIndex = 0;
        }
    }

    public MaterialData GetCurrentMaterial()
    {
        if (allMaterials.Length == 0) return null;
        return allMaterials[currentMaterialIndex];
    }

    public Sprite GetDisplayMaterialSprite()
    {
        var mat = GetCurrentMaterial();
        if (mat == null || mat.levelSprites == null || mat.levelSprites.Length == 0)
            return null;

        int index = currentVisualSpriteIndex % mat.levelSprites.Length;
        return mat.levelSprites[index];
    }

    public void AdvanceVisualSprite()
    {
        var mat = GetCurrentMaterial();
        if (mat == null || mat.levelSprites.Length == 0) return;

        currentVisualSpriteIndex = (currentVisualSpriteIndex + 1) % mat.levelSprites.Length;
    }

    // ----- UPGRADE -----

    public double GetCurrentMaterialUpgradeCost()
    {
        var mat = GetCurrentMaterial();
        if (mat == null) return 0;

        int tierMultiplier = currentMaterialIndex + 1;
        return mat.baseCost * (mat.currentLevel + 1) * tierMultiplier;
    }

    public bool UpgradeCurrentMaterial()
    {
        var mat = GetCurrentMaterial();
        if (mat == null || mat.currentLevel >= 10) return false;

        mat.currentLevel++;
        return true;
    }

    public int GetCurrentMaterialIndex()
    {
        return currentMaterialIndex;
    }

    // 🔘 Call this from Unity UI Button OnClick
    public void OnUpgradeMaterialButtonClicked()
    {
        MaterialData currentMaterial = GetCurrentMaterial();
        if (currentMaterial == null) return;

        double upgradeCost = GetCurrentMaterialUpgradeCost(); // ✅ use double

        double currentCoins = ShapeManager.Instance.GetCoinCount();

        if (currentCoins < upgradeCost)
        {
            Debug.Log("❌ Not enough coins to upgrade this material.");
            return;
        }

        bool upgraded = UpgradeCurrentMaterial();
        if (!upgraded)
        {
            Debug.Log("⚠️ Material already at max level.");
            return;
        }

        // Subtract coins + update UI
        ShapeManager.Instance.SpendCoins(upgradeCost);

        Debug.Log($"✅ {currentMaterial.materialName} upgraded to level {currentMaterial.currentLevel}. Coins left: {ShapeManager.Instance.GetCoinCount()}");

        // Refresh shape (in case health/coin multiplier changes)
        ShapeManager.Instance.LoadShapeFromSave(ShapeManager.Instance.GetCurrentShapeIndex());

        // UI update fallback (in case ShapeManager didn't trigger it)
        if (uiManager != null)
        {
            uiManager.UpdateCoinText(ShapeManager.Instance.GetCoinCount());
        }
    }




    // ----- STAT MULTIPLIERS -----

    public float GetHealthMultiplier()
    {
        var mat = GetCurrentMaterial();
        return mat != null ? 1f + mat.currentLevel * 0.25f : 1f;
    }

    public float GetCoinMultiplier()
    {
        var mat = GetCurrentMaterial();
        return mat != null ? 1f + mat.currentLevel * 0.20f : 1f;
    }
}
