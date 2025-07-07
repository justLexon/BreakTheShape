//Show selection for current active shapes.

using UnityEngine;
using UnityEngine.UI;

public class ShapeIconUI : MonoBehaviour
{
    public Image iconImage;
    public GameObject activeOutline;
    private string shapeID;

    public void Setup(string id, Sprite sprite)
    {
        shapeID = id;
        iconImage.sprite = sprite;
        UpdateVisual();
    }

    public void OnToggleClicked()
    {
        bool nowActive = InventoryManager.Instance.ToggleShape(shapeID);
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bool active = InventoryManager.Instance.IsShapeActive(shapeID);
        activeOutline.SetActive(active);
    }
}
