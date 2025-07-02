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
        PlayerPrefs.SetInt("Coins", ShapeManager.Instance.coinCount);
        PlayerPrefs.SetInt("ShapesBroken", ShapeManager.Instance.shapesBrokenCounter);
        PlayerPrefs.SetInt("CurrentShapeIndex", ShapeManager.Instance.GetCurrentShapeIndex());
        PlayerPrefs.Save();

        Debug.Log("✅ Game Saved");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            ShapeManager.Instance.tapDamage = PlayerPrefs.GetInt("TapDamage");
            ShapeManager.Instance.coinCount = PlayerPrefs.GetInt("Coins");
            ShapeManager.Instance.shapesBrokenCounter = PlayerPrefs.GetInt("ShapesBroken");
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
