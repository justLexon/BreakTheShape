using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShapePackBuyButton : MonoBehaviour
{
    public ShapePack shapePack;
    public ShapePopupUI shapePopupUI;
    public TMP_Text costText; // Assign in Inspector
    public double costMultiplier;

    void Start()
    {
        UpdateCostText();
    }

   public void OnBuyPressed()
{
    int amountToBuy = ShapePackBuyAmountSetter.BuyAmount;
    int shapesNeeded = shapePack.shapes.Count(s => !SaveManager.Instance.IsShapeOwned(s.id));

    if (shapesNeeded == 0)
    {
        shapePopupUI.ShowMessage("You already own all shapes in this pack!");
        return;
    }

    int purchasesMade = 0;
    int uniqueShapesGained = 0;
    double totalSpent = 0;
    List<double> usedPackCosts = new();

    for (int i = 0; i < amountToBuy; i++)
    {
        double currentCurrency = CurrencySelector.UsePremium ? ShapeManager.Instance.premiumCoinCount : ShapeManager.Instance.coinCount;

        if (currentCurrency < shapePack.cost)
        {
            if (i == 0)
                shapePopupUI.ShowMessage("❌ Not enough coins to buy this pack.");
            break;
        }

        if (uniqueShapesGained >= shapesNeeded)
            break;

        double thisPackCost = shapePack.cost;

        // Spend coins
        if (CurrencySelector.UsePremium)
            ShapeManager.Instance.premiumCoinCount -= thisPackCost;
        else
            ShapeManager.Instance.coinCount -= thisPackCost;

        totalSpent += thisPackCost;
        usedPackCosts.Add(thisPackCost);
        purchasesMade++;

        // Update cost and UI
        shapePack.cost = System.Math.Round(shapePack.cost * costMultiplier);
        ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
        SaveSystem.Instance.SaveProgress();
        UpdateCostText();

        ShapeItem shape = shapePack.GetRandomShape();
        if (shape == null) continue;

        bool success = SaveManager.Instance.AddShapeToOwned(shape.id);
        if (success)
        {
            uniqueShapesGained++;
            shapePopupUI.EnqueueReward(shape.icon, shape.id);
        }
        else
        {
            double refund = System.Math.Ceiling(thisPackCost * 0.25);
            if (CurrencySelector.UsePremium)
                ShapeManager.Instance.premiumCoinCount += refund;
            else
                ShapeManager.Instance.coinCount += refund;

            ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
            SaveSystem.Instance.SaveProgress();
            shapePopupUI.EnqueueRefund((float)refund);
        }
    }

    // Refund unused full packs
    int unusedPacks = amountToBuy - purchasesMade;
    double unusedRefund = 0;

    for (int i = purchasesMade; i < amountToBuy; i++)
    {
        shapePack.cost = System.Math.Floor(shapePack.cost / costMultiplier);
        unusedRefund += shapePack.cost;
    }

    if (unusedRefund > 0)
    {
        if (CurrencySelector.UsePremium)
            ShapeManager.Instance.premiumCoinCount += unusedRefund;
        else
            ShapeManager.Instance.coinCount += unusedRefund;

        ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
        SaveSystem.Instance.SaveProgress();

        shapePopupUI.EnqueueRefund((float)unusedRefund, $"Refunded {unusedRefund} for unused packs");
    }

    shapePopupUI.ShowNextInQueue();
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
