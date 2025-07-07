using UnityEngine;

public class ShapePackUI : MonoBehaviour
{
    public Transform container; // Content of ScrollRect
    public GameObject iconPrefab; // ShapeIconUI prefab

    public void PopulatePack(ShapePack pack)
    {
        foreach (var shape in pack.shapes)
        {
            var iconGO = Instantiate(iconPrefab, container);
            var icon = iconGO.GetComponent<ShapeIconUI>();
            icon.Setup(shape.shapeID, shape.shapeSprite);
        }
    }
}
