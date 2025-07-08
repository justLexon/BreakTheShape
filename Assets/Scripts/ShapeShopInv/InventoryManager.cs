using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private HashSet<string> enabledShapes = new();

    void Awake()
    {  
        Instance = this;
    }

    public void OnShapeToggled(ShapeItem shape)
    {
        if (shape.isEnabled)
        {
            enabledShapes.Add(shape.id);
        }
        else
        {
            enabledShapes.Remove(shape.id);
        }

        if (enabledShapes.Count > 10)
        {
            shape.isEnabled = false;
            enabledShapes.Remove(shape.id);
            Debug.Log("❌ Can't enable more than 10 shapes.");
        }

        Debug.Log($"✅ Enabled Shapes: {enabledShapes.Count}");
    }
}
