//Manage the inventory.

using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public int maxActiveShapes = 10;
    public List<ShapePack> shapePacks;

    private HashSet<string> activeShapeIDs = new();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public bool ToggleShape(string shapeID)
    {
        if (activeShapeIDs.Contains(shapeID))
        {
            activeShapeIDs.Remove(shapeID);
            return false;
        }
        else if (activeShapeIDs.Count < maxActiveShapes)
        {
            activeShapeIDs.Add(shapeID);
            return true;
        }

        Debug.Log("❌ Max active shapes reached");
        return false;
    }

    public bool IsShapeActive(string shapeID) => activeShapeIDs.Contains(shapeID);
    public int GetActiveCount() => activeShapeIDs.Count;
}
