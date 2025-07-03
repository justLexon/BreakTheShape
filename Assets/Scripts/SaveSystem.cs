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
        DontDestroyOnLoad(gameObject); // Optional: Keep SaveSystem alive between scenes
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("TapDamage", ShapeManager.Instance.tapDamage);
        PlayerPrefs.SetString("Coins", ShapeManager.Instance.coinCount.ToString("R")); // Save double
        PlayerPrefs.SetString("ShapesBroken", ShapeManager.Instance.shapesBrokenCounter.ToString("R")); // Save double
        PlayerPrefs.SetInt("CurrentShapeIndex", ShapeManager.Instance.GetCurrentShapeIndex());

        // Save current material index from MaterialManager
        if (MaterialManager.Instance != null)
        {
            PlayerPrefs.SetInt("CurrentMaterialIndex", MaterialManager.Instance.GetCurrentMaterialIndex());

            // Save levels of all materials
            for (int i = 0; i < MaterialManager.Instance.allMaterials.Length; i++)
            {
                PlayerPrefs.SetInt($"MaterialLevel_{i}", MaterialManager.Instance.allMaterials[i].currentLevel);
            }
        }

        PlayerPrefs.Save();
        Debug.Log("✅ Game Saved");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            ShapeManager.Instance.tapDamage = PlayerPrefs.GetInt("TapDamage");

            string savedCoinString = PlayerPrefs.GetString("Coins");
            if (double.TryParse(savedCoinString, out double loadedCoins))
                ShapeManager.Instance.coinCount = loadedCoins;
            else
                ShapeManager.Instance.coinCount = 0;

            string savedShapesString = PlayerPrefs.GetString("ShapesBroken");
            if (double.TryParse(savedShapesString, out double loadedShapes))
                ShapeManager.Instance.shapesBrokenCounter = loadedShapes;
            else
                ShapeManager.Instance.shapesBrokenCounter = 0;

            int shapeIndex = PlayerPrefs.GetInt("CurrentShapeIndex");
            ShapeManager.Instance.LoadShapeFromSave(shapeIndex);

            // Load current material index and apply it
            if (MaterialManager.Instance != null)
            {
                int savedMaterialIndex = PlayerPrefs.GetInt("CurrentMaterialIndex", 0);
                MaterialManager.Instance.SetCurrentMaterial(savedMaterialIndex);

                // Load levels for all materials
                for (int i = 0; i < MaterialManager.Instance.allMaterials.Length; i++)
                {
                    int level = PlayerPrefs.GetInt($"MaterialLevel_{i}", 0);
                    MaterialManager.Instance.allMaterials[i].currentLevel = level;
                }
            }

            Debug.Log("✅ Game Loaded");
        }
        else
        {
            Debug.Log("📦 No saved data found, loading default.");
            ShapeManager.Instance.LoadShapeFromSave(0);
            if (MaterialManager.Instance != null)
                MaterialManager.Instance.SetCurrentMaterial(0);
        }
    }


    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("🔄 Save data reset.");
    }
}
