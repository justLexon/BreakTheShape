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
                    mat.upgradePower = 8;
                    mat.coinUpgrade = 1; //C 15 ; Max 202 ; Avg 13
                    break;
                case "Wood":
                    mat.baseCost = 500;
                    mat.costMultiplier = 1.25;
                    mat.upgradePower = 50;
                    mat.coinUpgrade = 5; //C 65 ; Max 4.7k ; Avg 72
                    break;
                case "Stone":
                    mat.baseCost = 10000;
                    mat.costMultiplier = 1.2;
                    mat.upgradePower = 750;
                    mat.coinUpgrade = 45; //C 515 ; Max 61.9k ; Avg 120  **Mission1 Completed***
                    break;
                case "Granite":
                    mat.baseCost = 125000;
                    mat.costMultiplier = 1.15;
                    mat.upgradePower = 1500;
                    mat.coinUpgrade = 150; //C 2015 ; Max 505.7k ; Avg 250
                    break;
                case "Marble":
                    mat.baseCost = 1000000;
                    mat.costMultiplier = 1.15;
                    mat.upgradePower = 20000;
                    mat.coinUpgrade = 600; //C 8015 ; Max 4M ; Avg 500
                    break;
                case "Bronze":
                    mat.baseCost = 8000000;
                    mat.costMultiplier = 1.1;
                    mat.upgradePower = 50000;
                    mat.coinUpgrade = 10000; //C 108015 ; Max 20.7M ; Avg 985; coinsOriginal 1300
                    break;
                case "Silver":
                    mat.baseCost = 250000000;
                    mat.costMultiplier = 1.1;
                    mat.upgradePower = 150000;
                    mat.coinUpgrade = 35000; //C 52015 ; Max 129.7M ; Avg 2500; 3100
                    break;
                case "Gold":
                    mat.baseCost = 3000000000;
                    mat.costMultiplier = 1.09;
                    mat.upgradePower = 500000;
                    mat.coinUpgrade = 200000; //C 118015 ; Max 591.8M ; Avg 5000; 6600
                    break;
                case "Jade":
                    mat.baseCost = 50000000000;
                    mat.costMultiplier = 1.4;
                    mat.upgradePower = 5000000;
                    mat.coinUpgrade = 10000000; //C 490015 ; Max 4.9B ; Avg 10000; 37200
                    break;
                case "Diamond":
                    mat.baseCost = 5000000000000;
                    mat.costMultiplier = 1.35;
                    mat.upgradePower = 20000000;
                    mat.coinUpgrade = 200000000; //C 6.07M; Max 121.4B ; Avg 20000; 600000
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
        currentMaterialIndex = Mathf.Clamp(index, 0, materials.Length);
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
