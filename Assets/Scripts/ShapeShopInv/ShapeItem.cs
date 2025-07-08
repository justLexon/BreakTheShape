using UnityEngine;

[CreateAssetMenu(fileName = "NewShapeItem", menuName = "Shapes/Shape Item")]
public class ShapeItem : ScriptableObject
{
    public string id;
    public Sprite icon;

    [HideInInspector] public bool isUnlocked = false;
    [HideInInspector] public bool isEnabled = false;
}
