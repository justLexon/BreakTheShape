using TMPro;
using UnityEngine;

public class ShapePackBuyButton : MonoBehaviour
{
    public ShapePack shapePack;
    public ShapePopupUI shapePopupUI;
    public TMP_Text costText; // Assign in Inspector

    void Start()
    {
        UpdateCostText();
    }

    public void OnBuyPressed()
    {
        if (AllShapesOwnedInPack())
        {
            Debug.Log("🙅 You already own all shapes in this pack!");
            shapePopupUI.ShowMessage("You already own all shapes in this pack!");
            return;
        }

        double cost = shapePack.cost;

        if (ShapeManager.Instance.coinCount < cost)
        {
            Debug.Log("❌ Not enough coins to buy this pack.");
            return;
        }

        // Deduct coins
        ShapeManager.Instance.coinCount -= cost;
        ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
        SaveSystem.Instance.SaveProgress();

        // Increase cost by 10%
        shapePack.cost *= 1.1;
        UpdateCostText();

        // Pick random shape
        ShapeItem randomShape = shapePack.GetRandomShape();
        if (randomShape == null)
        {
            Debug.LogWarning("No shapes in pack!");
            return;
        }

        bool success = SaveManager.Instance.AddShapeToOwned(randomShape.id);
        if (success)
        {
            Debug.Log($"🎉 You got: {randomShape.id}");
            shapePopupUI.Show(randomShape.icon, randomShape.id);
        }
        else
        {
            double refund = cost * 0.25;
            ShapeManager.Instance.coinCount += refund;
            ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
            SaveSystem.Instance.SaveProgress();

            Debug.Log($"💰 Duplicate! Refunding 1/4 of cost for: {randomShape.id}");
            shapePopupUI.ShowDuplicateRefund((float)refund);
        }
    }

    private void UpdateCostText()
    {
        if (costText != null)
        {
            costText.text = FormatNumberWithSuffix(shapePack.cost);
        }
    }

    private string FormatNumberWithSuffix(double number)
    {
        if (number < 1000)
            return number.ToString("0");

        string[] suffixes = {
            "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
        };

        int suffixIndex = -1;
        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }

        return number.ToString("0.#") + suffixes[suffixIndex];
    }

    private bool AllShapesOwnedInPack()
    {
        foreach (var shape in shapePack.shapes)
        {
            if (!SaveManager.Instance.IsShapeOwned(shape.id))
                return false;
        }
        return true;
    }
}
