using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public int maxActiveShapes = 10;
    public List<ShapeItem> allShapes = new List<ShapeItem>();

    public void EnableShape(ShapeItem item)
    {
        int activeCount = allShapes.FindAll(s => s.isEnabled).Count;

        if (activeCount < maxActiveShapes)
        {
            item.isEnabled = true;
        }
        else
        {
            Debug.Log("Cannot enable more than 10 shapes.");
        }
    }

    public void DisableShape(ShapeItem item)
    {
        int activeCount = allShapes.FindAll(s => s.isEnabled).Count;

        if (activeCount > 10)
        {
            item.isEnabled = false;
        }
        else
        {
            Debug.Log("You must have exactly 10 shapes enabled.");
        }
    }
}
