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
        PlayerPrefs.SetInt("TapDamage", ShapeManager.Instance.tapDamage);
        PlayerPrefs.SetString("Coins", ShapeManager.Instance.coinCount.ToString("R"));
        PlayerPrefs.SetString("ShapesBroken", ShapeManager.Instance.shapesBrokenCounter.ToString("R"));
        PlayerPrefs.SetInt("CurrentShapeIndex", ShapeManager.Instance.GetCurrentShapeIndex());
        PlayerPrefs.SetInt("BaseMaxHealth", ShapeManager.Instance.baseMaxHealth);

        if (MaterialsManager.Instance != null)
        {
            PlayerPrefs.SetInt("CurrentMaterialIndex", MaterialsManager.Instance.GetCurrentMaterialIndex());
            PlayerPrefs.SetInt("TextureCycleIndex", MaterialsManager.Instance.GetTextureCycleIndex()); // ✅ save texture index

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
            ShapeManager.Instance.tapDamage = PlayerPrefs.GetInt("TapDamage", 1);

            string savedCoinString = PlayerPrefs.GetString("Coins");
            ShapeManager.Instance.coinCount = double.TryParse(savedCoinString, out double coins) ? coins : 0;

            string savedShapesString = PlayerPrefs.GetString("ShapesBroken");
            ShapeManager.Instance.shapesBrokenCounter = double.TryParse(savedShapesString, out double broken) ? broken : 0;

            ShapeManager.Instance.baseMaxHealth = PlayerPrefs.GetInt("BaseMaxHealth", 10);

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

            // **Call LoadShapeFromSave only after MaterialsManager indices are set!**
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
