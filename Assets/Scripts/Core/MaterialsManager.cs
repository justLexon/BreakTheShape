using UnityEngine;
using System.Linq;

[System.Serializable]
public class MaterialData
{
    public string materialName;
    public Sprite[] levelSprites;

    [Header("Upgrade Settings")]
    public int currentLevel = 0;
    public double baseCost = 50;

    [Tooltip("Multiplier for each upgrade cost step")]
    public double costMultiplier = 1.15;

    public int upgradePower = 1;
    public int coinUpgrade = 1;
}


public class MaterialsManager : MonoBehaviour
{
    public static MaterialsManager Instance { get; private set; }

    [Header("Materials List")]
    public MaterialData[] materials;

    private int textureCycleIndex = 0;


    public bool IsInitialized { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate MaterialsManager destroyed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("MaterialsManager Awake - instance set");

        LoadAllMaterialLevels();

        InitializeBaseCostsAndPowers();

        currentMaterialIndex = PlayerPrefs.GetInt("CurrentMaterialIndex", 0);

        IsInitialized = true;  // signal ready
    }



    // Set baseCosts and upgradePower by switch-case based on materialName
    void InitializeBaseCostsAndPowers()
    {
        foreach (var mat in materials)
        {
            switch (mat.materialName)
            {
                case "Dirt":
                    mat.baseCost = 50;
                    mat.costMultiplier = 1.15;
                    mat.upgradePower = 3;
                    mat.coinUpgrade = 1;
                    break;
                case "Wood":
                    mat.baseCost = 250;
                    mat.costMultiplier = 1.2;
                    mat.upgradePower = 10;
                    mat.coinUpgrade = 50;
                    break;
                case "Stone":
                    mat.baseCost = 500;
                    mat.costMultiplier = 1.25;
                    mat.upgradePower = 45;
                    mat.coinUpgrade = 100;
                    break;
                case "Granite":
                    mat.baseCost = 1000;
                    mat.costMultiplier = 1.3;
                    mat.upgradePower = 80;
                    mat.coinUpgrade = 300;
                    break;
                case "Marble":
                    mat.baseCost = 5000;
                    mat.costMultiplier = 1.35;
                    mat.upgradePower = 220;
                    mat.coinUpgrade = 1000;
                    break;
                case "Bronze":
                    mat.baseCost = 50000;
                    mat.costMultiplier = 1.4;
                    mat.upgradePower = 550;
                    mat.coinUpgrade = 5000;
                    break;
                case "Silver":
                    mat.baseCost = 100000;
                    mat.costMultiplier = 1.4;
                    mat.upgradePower = 1550;
                    mat.coinUpgrade = 15000;
                    break;
                case "Gold":
                    mat.baseCost = 250000;
                    mat.costMultiplier = 1.4;
                    mat.upgradePower = 5550;
                    mat.coinUpgrade = 50000;
                    break;
                case "Jade":
                    mat.baseCost = 500000;
                    mat.costMultiplier = 1.4;
                    mat.upgradePower = 10550;
                    mat.coinUpgrade = 100000;
                    break;
                case "Diamond":
                    mat.baseCost = 1000000;
                    mat.costMultiplier = 1.4;
                    mat.upgradePower = 50550;
                    mat.coinUpgrade = 1000000;
                    break;
                default:
                    mat.baseCost = 100;
                    mat.costMultiplier = 1.15;
                    mat.upgradePower = 1;
                    mat.coinUpgrade = 1;
                    break;
            }
        }
    }


    public MaterialData GetMaterial(string key)
    {
        return materials.FirstOrDefault(m => m.materialName == key);
    }

    public int GetMaterialLevel(string key)
    {
        var mat = GetMaterial(key);
        return mat != null ? mat.currentLevel : 0;
    }

    public void SetMaterialLevel(string key, int level)
    {
        var mat = GetMaterial(key);
        if (mat != null)
        {
            mat.currentLevel = level;
            PlayerPrefs.SetInt(key + "_Level", level);
            PlayerPrefs.Save();
        }
    }

    public void AdvanceTextureCycle()
    {
        textureCycleIndex++;
    }

    public int GetTextureCycleIndex() => textureCycleIndex;
    public void SetTextureCycleIndex(int value) => textureCycleIndex = value;

    public Sprite GetDisplayMaterialSprite()
    {
        var mat = materials[GetCurrentMaterialIndex()];

        if (mat.levelSprites != null && mat.levelSprites.Length > 0)
        {
            textureCycleIndex %= mat.levelSprites.Length;
            Sprite result = mat.levelSprites[textureCycleIndex];
            Debug.Log($"GetDisplayMaterialSprite() returning sprite '{result.name}' from material '{mat.materialName}' at index {textureCycleIndex}");
            return result;
        }

        Debug.LogWarning($"GetDisplayMaterialSprite() - No sprites found for material '{mat.materialName}'");
        return null;
    }


    private int currentMaterialIndex = 0;

    public int GetCurrentMaterialIndex() => currentMaterialIndex;

    public void SetCurrentMaterial(int index)
    {
        currentMaterialIndex = Mathf.Clamp(index, 0, materials.Length - 1);
        Debug.Log($"✅ SetCurrentMaterial to: {materials[currentMaterialIndex].materialName} (index {currentMaterialIndex})");
    }


    public void AutoSelectHighestUnlockedMaterial()
    {
        for (int i = materials.Length - 1; i >= 0; i--)
        {
            if (materials[i].currentLevel > 0)
            {
                SetCurrentMaterial(i);
                return;
            }
        }

        // fallback to Dirt if nothing is upgraded
        SetCurrentMaterial(0);
    }

    public int GetMaterialIndex(string key)
    {
        return System.Array.FindIndex(materials, m => m.materialName == key);
    }



    public double GetUpgradeCost(string key)
    {
        MaterialData mat = GetMaterial(key);
        if (mat == null) return 0;

        // Use material's baseCost and costMultiplier directly
        double cost = mat.baseCost * System.Math.Pow(mat.costMultiplier, mat.currentLevel);
        return System.Math.Round(cost);
    }


    public int GetUpgradePower(string key)
    {
        var mat = GetMaterial(key);
        return mat != null ? mat.upgradePower : 0;
    }

    public int GetCoinUpgrade(string key)
    {
        var mat = GetMaterial(key);
        return mat != null ? mat.coinUpgrade : 0;
    }

    public bool CanUpgrade(string key, int maxLevel)
    {
        var mat = GetMaterial(key);
        if (mat == null) return false;
        return mat.currentLevel < maxLevel;
    }

    public void UpgradeMaterial(string key)
    {
        var mat = GetMaterial(key);
        if (mat != null && mat.currentLevel < 999)
        {
            mat.currentLevel++;
            SetMaterialLevel(key, mat.currentLevel);
        }
    }

    void LoadAllMaterialLevels()
    {
        foreach (var mat in materials)
        {
            mat.currentLevel = PlayerPrefs.GetInt(mat.materialName + "_Level", 0);
        }
    }

    public double GetUpgradeCostAtLevel(string key, int level)
    {
        var mat = GetMaterial(key);
        if (mat == null) return 0;

        return System.Math.Round(mat.baseCost * System.Math.Pow(mat.costMultiplier, level));
    }


}
