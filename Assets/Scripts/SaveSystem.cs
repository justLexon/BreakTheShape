using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetString("Coins", ShapeManager.Instance.coinCount.ToString("R"));
        PlayerPrefs.SetString("CoinsPerBreak", ShapeManager.Instance.coinsPerBreak.ToString("R"));
        PlayerPrefs.SetString("ShapesBroken", ShapeManager.Instance.shapesBrokenCounter.ToString("R"));
        PlayerPrefs.SetInt("CurrentShapeIndex", ShapeManager.Instance.GetCurrentShapeIndex());

        PlayerPrefs.SetString("TapDamage", ShapeManager.Instance.tapDamage.ToString("R"));         // ✅ save double
        PlayerPrefs.SetString("IdleDamage", ShapeManager.Instance.idleDamagePerSecond.ToString("R"));         // ✅ save double
        PlayerPrefs.SetString("BaseMaxHealth", ShapeManager.Instance.baseMaxHealth.ToString("R")); 

        if (MaterialsManager.Instance != null)
        {
            PlayerPrefs.SetInt("CurrentMaterialIndex", MaterialsManager.Instance.GetCurrentMaterialIndex());
            PlayerPrefs.SetInt("TextureCycleIndex", MaterialsManager.Instance.GetTextureCycleIndex());

            foreach (var mat in MaterialsManager.Instance.materials)
            {
                string key = $"{mat.materialName}_Level";
                PlayerPrefs.SetInt(key, mat.currentLevel);
            }
        }

        PlayerPrefs.Save();
        Debug.Log("✅ Game Saved");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            // Load doubles safely
            string savedTapDamage = PlayerPrefs.GetString("TapDamage", "1");
            ShapeManager.Instance.tapDamage = double.TryParse(savedTapDamage, out double td) ? td : 1;

            string savedBaseMaxHealth = PlayerPrefs.GetString("BaseMaxHealth", "10");
            ShapeManager.Instance.baseMaxHealth = double.TryParse(savedBaseMaxHealth, out double bmh) ? bmh : 10;

            string savedCoinString = PlayerPrefs.GetString("Coins");
            ShapeManager.Instance.coinCount = double.TryParse(savedCoinString, out double coins) ? coins : 0;

            string savedCoinsPerBreakString = PlayerPrefs.GetString("CoinsPerBreak");
            ShapeManager.Instance.coinsPerBreak = double.TryParse(savedCoinsPerBreakString, out double coinsBreak) ? coinsBreak : 1;

            string savedShapesString = PlayerPrefs.GetString("ShapesBroken");
            ShapeManager.Instance.shapesBrokenCounter = double.TryParse(savedShapesString, out double broken) ? broken : 0;

            string savedIdleDamageString = PlayerPrefs.GetString("IdleDamage");
            ShapeManager.Instance.idleDamagePerSecond = double.TryParse(savedIdleDamageString, out double idDam) ? idDam : 1;

            if (MaterialsManager.Instance != null)
            {
                foreach (var mat in MaterialsManager.Instance.materials)
                {
                    string key = $"{mat.materialName}_Level";
                    mat.currentLevel = PlayerPrefs.GetInt(key, 0);
                }

                int savedIndex = PlayerPrefs.GetInt("CurrentMaterialIndex", 0);
                Debug.Log($"LoadProgress: setting currentMaterialIndex = {savedIndex}");
                MaterialsManager.Instance.SetCurrentMaterial(savedIndex);

                int textureIndex = PlayerPrefs.GetInt("TextureCycleIndex", 0);
                Debug.Log($"LoadProgress: setting textureCycleIndex = {textureIndex}");
                MaterialsManager.Instance.SetTextureCycleIndex(textureIndex);
            }

            int shapeIndex = PlayerPrefs.GetInt("CurrentShapeIndex", 0);
            ShapeManager.Instance.LoadShapeFromSave(shapeIndex);

            Debug.Log("✅ Game Loaded");
        }
        else
        {
            Debug.Log("📦 No saved data found, loading default.");
            if (MaterialsManager.Instance != null)
            {
                MaterialsManager.Instance.AutoSelectHighestUnlockedMaterial();
            }
            ShapeManager.Instance.LoadShapeFromSave(0);
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("🔄 Save data reset.");
    }
}
