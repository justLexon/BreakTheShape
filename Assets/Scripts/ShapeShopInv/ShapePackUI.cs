using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShapePackUI : MonoBehaviour
{
    public TMP_Text packNameText;
    public Transform shapeGrid;
    public GameObject shapeIconPrefab;

    public void Setup(ShapePack pack)
    {
        packNameText.text = pack.packName;

        foreach (Transform child in shapeGrid)
            Destroy(child.gameObject); // Clear old

        foreach (ShapeItem shape in pack.shapes)
        {
            GameObject iconGO = Instantiate(shapeIconPrefab, shapeGrid);
            ShapeIconUI iconUI = iconGO.GetComponent<ShapeIconUI>();
            iconUI.Setup(shape);
        }
    }
}
