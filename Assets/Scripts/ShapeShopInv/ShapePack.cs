using UnityEngine;

[CreateAssetMenu(fileName = "NewShapePack", menuName = "Shapes/Shape Pack")]
public class ShapePack : ScriptableObject
{
    public string packName;
    public string packId;
    public double cost;
    public ShapeItem[] shapes;

    public ShapeItem GetRandomShape()
    {
        if (shapes == null || shapes.Length == 0)
            return null;

        return shapes[Random.Range(0, shapes.Length)];
    }
}
