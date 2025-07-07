using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Shape Pack")]
public class ShapePack : ScriptableObject
{
    public string packName;
    public ShapeItem[] shapes;
}
