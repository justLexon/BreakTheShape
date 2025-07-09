using UnityEngine;

[CreateAssetMenu(fileName = "NewShapeItem", menuName = "Shapes/Shape Item")]
public class ShapeItem : ScriptableObject
{
    public string id;
    public Sprite icon;

    public bool isUnlocked = false;
    public bool isEnabled = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            id = name; // Auto-assign the ScriptableObject's filename as ID
        }
    }
#endif

}
