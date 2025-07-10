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

        //LoadOwnedShapes();
    }

    public bool AddShapeToOwned(string id)
    {
        Debug.Log($"🧪 Trying to add shape: {id}");

        if (ownedShapeIds.Contains(id))
        {
            Debug.Log($"❌ Duplicate shape: {id}");
            return false;
        }

        ownedShapeIds.Add(id);
        SaveOwnedShapes();
        Debug.Log($"✅ New shape added: {id}");
        DebugOwnedShapes();
        return true;
    }


    public void SaveOwnedShapes()
    {
        string saveString = string.Join(",", ownedShapeIds);
        PlayerPrefs.SetString("OwnedShapes", saveString);
        PlayerPrefs.Save();
    }

    public void LoadOwnedShapes()
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
        //Debug.Log($"📦 Loaded shapes: {saveString}");

    }

    public void DebugOwnedShapes()
    {
        //if (ownedShapeIds.Count == 0)
        //{
        //    Debug.Log("📭 You don't own any shapes yet.");
        //    return;
        //}

        //Debug.Log("🧾 Owned Shapes:");
        //foreach (string id in ownedShapeIds)
        //{
        //    Debug.Log($"🔹 {id}");
        //}
        return;
    }

    public bool IsShapeOwned(string id)
    {
        return ownedShapeIds.Contains(id);
    }


}
