using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Mythic
}

[CreateAssetMenu(fileName = "NewShapeItem", menuName = "Shapes/Shape Item")]
public class ShapeItem : ScriptableObject
{
    public string id;
    public Sprite icon;
    public Rarity rarity;

    public bool isUnlocked = false;
    public bool isEnabled = false;

    public float GetDropChance()
    {
        return rarity switch
        {
            Rarity.Common => 50f,
            Rarity.Rare => 30f,
            Rarity.Epic => 15f,
            Rarity.Legendary => 4f,
            Rarity.Mythic => 1f,
            _ => 0f
        };
    }

    public Color GetRarityColor()
    {
        return rarity switch
        {
            Rarity.Common => HexToColor("8882AA"),
            Rarity.Rare => HexToColor("052E96"),
            Rarity.Epic => HexToColor("7C0098"),
            Rarity.Legendary => HexToColor("BFA00B"),
            Rarity.Mythic => HexToColor("850404"),
            _ => Color.white
        };
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
            return color;

        return Color.white;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            id = name;
        }
    }
#endif
}
