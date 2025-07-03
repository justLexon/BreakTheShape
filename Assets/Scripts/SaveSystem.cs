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

        if (MaterialsManager.Instance != null)
        {
            // Save levels of all materials by material name
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

            int shapeIndex = PlayerPrefs.GetInt("CurrentShapeIndex", 0);
            ShapeManager.Instance.LoadShapeFromSave(shapeIndex);

            if (MaterialsManager.Instance != null)
            {
                foreach (var mat in MaterialsManager.Instance.materials)
                {
                    string key = $"{mat.materialName}_Level";
                    mat.currentLevel = PlayerPrefs.GetInt(key, 0);
                }
            }

            Debug.Log("✅ Game Loaded");
        }
        else
        {
            Debug.Log("📦 No saved data found, loading default.");
            ShapeManager.Instance.LoadShapeFromSave(0);
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("🔄 Save data reset.");
    }
}
