using UnityEngine;

[CreateAssetMenu(fileName = "NewShapePack", menuName = "Shapes/Shape Pack")]
public class ShapePack : ScriptableObject
{
    public string packName;
    public string packId;
    public Sprite packSprite;     // ✅ Add this
    public double cost = 100;
    public ShapeItem[] shapes;

    public ShapeItem GetRandomShape()
    {
        if (shapes == null || shapes.Length == 0)
            return null;

        return shapes[Random.Range(0, shapes.Length)];
    }

    public bool AllShapesOwned()
    {
        foreach (var shape in shapes)
        {
            if (!SaveManager.Instance.IsShapeOwned(shape.id))
                return false;
        }
        return true;
    }
}
