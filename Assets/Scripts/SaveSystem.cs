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
        PlayerPrefs.Save();

        Debug.Log("✅ Game Saved");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            ShapeManager.Instance.tapDamage = PlayerPrefs.GetInt("TapDamage");

            // Load coins
            string savedCoinString = PlayerPrefs.GetString("Coins");
            if (double.TryParse(savedCoinString, out double loadedCoins))
                ShapeManager.Instance.coinCount = loadedCoins;
            else
                ShapeManager.Instance.coinCount = 0;

            // Load shapes broken
            string savedShapesString = PlayerPrefs.GetString("ShapesBroken");
            if (double.TryParse(savedShapesString, out double loadedShapes))
                ShapeManager.Instance.shapesBrokenCounter = loadedShapes;
            else
                ShapeManager.Instance.shapesBrokenCounter = 0;

            int index = PlayerPrefs.GetInt("CurrentShapeIndex");
            ShapeManager.Instance.LoadShapeFromSave(index);

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
