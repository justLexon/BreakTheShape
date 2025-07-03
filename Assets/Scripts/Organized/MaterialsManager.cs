using UnityEngine;
using System.Linq;

[System.Serializable]
public class MaterialData
{
    public string materialName;
    public Sprite[] levelSprites;      // optional for visuals
    public int currentLevel = 0;       // current upgrade level
    public double baseCost = 50;       // base cost for upgrades
    public int upgradePower = 1;       // power increment per level
}

public class MaterialsManager : MonoBehaviour
{
    public static MaterialsManager Instance { get; private set; }

    [Header("Materials List")]
    public MaterialData[] materials;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllMaterialLevels();
        InitializeBaseCostsAndPowers();
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
                    mat.upgradePower = 1;
                    break;
                case "Stone":
                    mat.baseCost = 250;
                    mat.upgradePower = 5;
                    break;
                case "Wood":
                    mat.baseCost = 500;
                    mat.upgradePower = 15;
                    break;
                case "Iron":
                    mat.baseCost = 1000;
                    mat.upgradePower = 50;
                    break;
                case "Gold":
                    mat.baseCost = 5000;
                    mat.upgradePower = 150;
                    break;
                case "Diamond":
                    mat.baseCost = 50000;
                    mat.upgradePower = 250;
                    break;
                default:
                    mat.baseCost = 100;
                    mat.upgradePower = 1;
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

    public Sprite GetDisplayMaterialSprite()
    {
        var mat = materials[currentMaterialIndex];

        if (mat.levelSprites != null && mat.levelSprites.Length > 0)
        {
            int index = Random.Range(0, mat.levelSprites.Length);
            return mat.levelSprites[index];
        }

        return null;
    }

    private int currentMaterialIndex = 0;

    public int GetCurrentMaterialIndex() => currentMaterialIndex;

    public void SetCurrentMaterial(int index)
    {
        currentMaterialIndex = Mathf.Clamp(index, 0, materials.Length - 1);
    }

    public int GetMaterialIndex(string key)
    {
        return System.Array.FindIndex(materials, m => m.materialName == key);
    }



    public double GetUpgradeCost(string key)
    {
        var mat = GetMaterial(key);
        if (mat == null) return 0;

        // exponential cost growth
        return Mathf.Round((float)(mat.baseCost * System.Math.Pow(1.15f, mat.currentLevel)));
    }

    public int GetUpgradePower(string key)
    {
        var mat = GetMaterial(key);
        return mat != null ? mat.upgradePower : 0;
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
}
