using UnityEngine;

public class ShapePackBuyButton : MonoBehaviour
{
    public ShapePack shapePack;
    public ShapePopupUI shapePopupUI; // Drag your popup UI reference here in Inspector

    public void OnBuyPressed()
    {
        ShapeItem randomShape = shapePack.GetRandomShape();

        if (randomShape == null)
        {
            Debug.LogWarning("No shapes in pack!");
            return;
        }

        bool success = true;
        if (success)
        {
            Debug.Log($"🎉 You got: {randomShape.id}");
            shapePopupUI.Show(randomShape.icon, randomShape.id);
        }
        else
        {
            Debug.Log($"💰 Duplicate! Refunding part of your coins for: {randomShape.id}");
        }
    }
}
