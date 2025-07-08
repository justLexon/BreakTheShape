using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private HashSet<string> ownedShapeIds = new HashSet<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadOwnedShapes();
    }

    public bool AddShapeToOwned(string id)
    {
        if (ownedShapeIds.Contains(id))
        {
            Debug.Log($"❌ Duplicate shape: {id}");
            return false;
        }

        ownedShapeIds.Add(id);
        SaveOwnedShapes();
        Debug.Log($"✅ New shape added: {id}");
        return true;
    }

    private void SaveOwnedShapes()
    {
        string saveString = string.Join(",", ownedShapeIds);
        PlayerPrefs.SetString("OwnedShapes", saveString);
        PlayerPrefs.Save();
    }

    private void LoadOwnedShapes()
    {
        ownedShapeIds.Clear();
        string saveString = PlayerPrefs.GetString("OwnedShapes", "");
        if (!string.IsNullOrEmpty(saveString))
        {
            string[] ids = saveString.Split(',');
            foreach (var id in ids)
            {
                ownedShapeIds.Add(id);
            }
        }
    }
}
