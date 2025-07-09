using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private HashSet<string> enabledShapes = new HashSet<string>();

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

    public bool IsShapeEnabled(string id)
    {
        return enabledShapes.Contains(id);
    }

    public int GetEnabledShapeCount()
    {
        return enabledShapes.Count;
    }

    public void SetShapeEnabled(string id, bool enable)
    {
        if (enable)
        {
            if (enabledShapes.Count >= 10)
            {
                Debug.Log("❌ You can only enable up to 10 shapes!");
                return;
            }
            enabledShapes.Add(id);
        }
        else
        {
            enabledShapes.Remove(id);
        }
    }

    // Use this from ShapeIconUI when toggling on click
    public void ToggleShape(string id, bool? forceEnable = null)
    {
        if (forceEnable.HasValue)
        {
            SetShapeEnabled(id, forceEnable.Value);
            return;
        }

        // Toggle behavior
        if (enabledShapes.Contains(id))
        {
            enabledShapes.Remove(id);
        }
        else
        {
            if (enabledShapes.Count >= 10)
            {
                Debug.Log("❌ Max 10 shapes can be enabled.");
                return;
            }
            enabledShapes.Add(id);
        }
    }

    public HashSet<string> GetEnabledShapes()
    {
        return enabledShapes;
    }

    public void ResetEnabledShapes()
    {
        enabledShapes.Clear();
    }

    public void EnableAllShapesInPack(ShapePack pack)
    {
        foreach (var shape in pack.shapes)
        {
            if (!enabledShapes.Contains(shape.id))
            {
                if (enabledShapes.Count < 10) // Respect max enabled shapes limit
                    enabledShapes.Add(shape.id);
                else
                    break; // Stop if max reached
            }
        }
    }

    public void OwnAndEnableAllShapesInPack(ShapePack pack)
    {
        foreach (var shape in pack.shapes)
        {
            if (!SaveManager.Instance.IsShapeOwned(shape.id))
            {
                SaveManager.Instance.AddShapeToOwned(shape.id);
            }
            if (!enabledShapes.Contains(shape.id))
            {
                if (enabledShapes.Count < 10)
                    enabledShapes.Add(shape.id);
                else
                    break;
            }
        }
    }

}
