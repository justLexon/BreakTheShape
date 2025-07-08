using UnityEngine;
using UnityEngine.UI;

public class ShapeIconUI : MonoBehaviour
{
    public Image shapeImage;
    public GameObject grayOverlay;
    public GameObject greenEnabledOverlay;

    private ShapeItem shapeData;

    public void Setup(ShapeItem shape)
    {
        shapeData = shape;
        shapeImage.sprite = shape.icon;

        // Gray out if locked
        grayOverlay.SetActive(!SaveManager.Instance.IsShapeOwned(shape.id));

        // Show green overlay if enabled
        greenEnabledOverlay.SetActive(shape.isEnabled);
    }

    public void ToggleEnable()
    {
        if (!SaveManager.Instance.IsShapeOwned(shapeData.id)) return;

        shapeData.isEnabled = !shapeData.isEnabled;
        greenEnabledOverlay.SetActive(shapeData.isEnabled);

        // Notify InventoryManager to enforce limit
        InventoryManager.Instance.OnShapeToggled(shapeData);
    }
}
