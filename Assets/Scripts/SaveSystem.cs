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

        var enabledShapeIds = InventoryManager.Instance.GetEnabledShapes();
        string enabledIdsSerialized = string.Join(",", enabledShapeIds);

        PlayerPrefs.SetString("EnabledShapes", enabledIdsSerialized);
        PlayerPrefs.Save();

        // --- Game Stats ---
        PlayerPrefs.SetString("Coins", ShapeManager.Instance.coinCount.ToString("R"));
        PlayerPrefs.SetString("premiumCoinCount", ShapeManager.Instance.premiumCoinCount.ToString());
        PlayerPrefs.SetString("CoinsPerBreak", ShapeManager.Instance.coinsPerBreak.ToString("R"));
        PlayerPrefs.SetString("ShapesBroken", ShapeManager.Instance.shapesBrokenCounter.ToString("R"));
        PlayerPrefs.SetInt("CurrentShapeIndex", ShapeManager.Instance.GetCurrentShapeIndex());
        PlayerPrefs.SetString("TapDamage", ShapeManager.Instance.tapDamage.ToString("R"));
        PlayerPrefs.SetString("IdleDamage", ShapeManager.Instance.idleDamagePerSecond.ToString("R"));
        PlayerPrefs.SetString("BaseMaxHealth", ShapeManager.Instance.baseMaxHealth.ToString("R"));

        // --- Materials ---
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

        // --- Owned Shapes ---
        SaveManager.Instance.SaveOwnedShapes();

        // --- Shape Pack Costs ---
        if (ShopManager.Instance != null)
        {
            foreach (var pack in ShopManager.Instance.allShapePacks)
            {
                PlayerPrefs.SetString($"PackCost_{pack.packId}", pack.cost.ToString("R"));
            }
        }

        PlayerPrefs.Save();
        Debug.Log("✅ Game Saved");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("EnabledShapes"))
        {
            string enabledIdsSerialized = PlayerPrefs.GetString("EnabledShapes");
            var enabledIds = enabledIdsSerialized.Split(',');

            InventoryManager.Instance.ResetEnabledShapes();
            foreach (var id in enabledIds)
            {
                if (!string.IsNullOrEmpty(id))
                    InventoryManager.Instance.SetShapeEnabled(id, true);
            }
        }

        if (PlayerPrefs.HasKey("Coins"))
        {
            // --- Game Stats ---
            ShapeManager.Instance.tapDamage = double.TryParse(PlayerPrefs.GetString("TapDamage", "1"), out double td) ? td : 1;
            ShapeManager.Instance.baseMaxHealth = double.TryParse(PlayerPrefs.GetString("BaseMaxHealth", "10"), out double bmh) ? bmh : 10;
            ShapeManager.Instance.coinCount = double.TryParse(PlayerPrefs.GetString("Coins"), out double coins) ? coins : 0;
            ShapeManager.Instance.premiumCoinCount = double.TryParse(PlayerPrefs.GetString("premiumCoinCount", "0"), out double pcoins) ? pcoins : 0;
            ShapeManager.Instance.coinsPerBreak = double.TryParse(PlayerPrefs.GetString("CoinsPerBreak"), out double coinsBreak) ? coinsBreak : 1;
            ShapeManager.Instance.shapesBrokenCounter = double.TryParse(PlayerPrefs.GetString("ShapesBroken"), out double broken) ? broken : 0;
            ShapeManager.Instance.idleDamagePerSecond = double.TryParse(PlayerPrefs.GetString("IdleDamage"), out double idDam) ? idDam : 1;

            // --- Materials ---
            if (MaterialsManager.Instance != null)
            {
                foreach (var mat in MaterialsManager.Instance.materials)
                {
                    string key = $"{mat.materialName}_Level";
                    mat.currentLevel = PlayerPrefs.GetInt(key, 0);
                }

                int savedIndex = PlayerPrefs.GetInt("CurrentMaterialIndex", 0);
                MaterialsManager.Instance.SetCurrentMaterial(savedIndex);

                int textureIndex = PlayerPrefs.GetInt("TextureCycleIndex", 0);
                MaterialsManager.Instance.SetTextureCycleIndex(textureIndex);
            }

            // --- Owned Shapes ---
            SaveManager.Instance.LoadOwnedShapes();

            // --- Shape Pack Costs ---
            if (ShopManager.Instance != null)
            {
                foreach (var pack in ShopManager.Instance.allShapePacks)
                {
                    string key = $"PackCost_{pack.packId}";
                    if (PlayerPrefs.HasKey(key))
                    {
                        string saved = PlayerPrefs.GetString(key);
                        pack.cost = double.TryParse(saved, out double result) ? result : pack.cost;
                    }
                }
            }

            // --- Current Shape ---
            int shapeIndex = PlayerPrefs.GetInt("CurrentShapeIndex", 0);
            ShapeManager.Instance.LoadShapeFromSave(shapeIndex);
            SaveManager.Instance.DebugOwnedShapes();
            ShapeManager.Instance.RefreshEnabledShapes(); // ✅ Refresh after loading

            Debug.Log("✅ Game Loaded");
        }
        else
        {
            Debug.Log("📦 No saved data found, loading default.");
            MaterialsManager.Instance?.AutoSelectHighestUnlockedMaterial();
            ShapeManager.Instance.LoadShapeFromSave(0);
        }
    }


    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();

        // 🛠️ Reset shape pack costs to default manually
        if (ShopManager.Instance != null)
        {
            foreach (var pack in ShopManager.Instance.allShapePacks)
            {
                pack.cost = 100; // Or any default starting value
            }
        }

        Debug.Log("🔄 Save data reset.");
    }
}
