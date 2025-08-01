﻿using System.Collections.Generic;
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

        // Step 1: Simulate possible purchases based on starting coin count
        double availableCoins = ShapeManager.Instance.coinCount;
        List<double> simulatedCosts = new();
        double tempCost = shapePack.cost;

        for (int i = 0; i < amountToBuy; i++)
        {
            if (availableCoins >= tempCost)
            {
                simulatedCosts.Add(tempCost);
                availableCoins -= tempCost;
                tempCost = System.Math.Round(tempCost * costMultiplier);
            }
            else
            {
                break;
            }
        }

        // Step 2: Actually purchase packs according to simulation
        int purchasesMade = 0;
        int uniqueShapesGained = 0;

        shapePopupUI.SetPackCounter(simulatedCosts.Count);

        foreach (double packCost in simulatedCosts)
        {
            if (uniqueShapesGained >= shapesNeeded)
                break;

            ShapeManager.Instance.coinCount -= packCost;
            ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
            purchasesMade++;

            BuySound.Instance.PlayBuySound();

            shapePack.cost = System.Math.Round(shapePack.cost * costMultiplier);
            SaveSystem.Instance.SaveProgress();
            UpdateCostText();

            ShapeItem shape = GetRandomShapeWeighted(shapePack.shapes.ToList()); // ✅ NEW
            if (shape == null) continue; //or continue

            bool success = SaveManager.Instance.AddShapeToOwned(shape.id);
            if (success)
            {
                uniqueShapesGained++;
                shapePopupUI.EnqueueReward(shape); // ✅ pass full shape
            }
            else
            {
                // Refund 25% for duplicates
                double refund = System.Math.Ceiling(packCost * 0.25);
                ShapeManager.Instance.coinCount += refund;
                ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
                SaveSystem.Instance.SaveProgress();
                shapePopupUI.EnqueueRefund((float)refund, shape);
            }
        }

        // Step 3: Refund unused packs from simulated list ONLY
        int unusedPacks = simulatedCosts.Count - purchasesMade;
        double unusedRefund = 0;

        for (int i = 0; i < unusedPacks; i++)
        {
            // Roll back cost for refund
            shapePack.cost = System.Math.Floor(shapePack.cost / costMultiplier);
            unusedRefund += simulatedCosts[simulatedCosts.Count - 1 - i]; // Last costs first
        }

        if (unusedRefund > 0)
        {
            ShapeManager.Instance.coinCount += unusedRefund;
            ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);
            SaveSystem.Instance.SaveProgress();
            shapePopupUI.EnqueueRefund((float)unusedRefund, $"Refunded {unusedRefund} coins for unused packs");
        }

        shapePopupUI.ShowNextInQueue();
    }


    private ShapeItem GetRandomShapeWeighted(List<ShapeItem> allShapes)
    {
        float totalWeight = 0f;
        List<(ShapeItem shape, float weight)> weightedList = new();

        foreach (var shape in allShapes)
        {
            float weight = shape.GetDropChance();
            weightedList.Add((shape, weight));
            totalWeight += weight;
        }

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var (shape, weight) in weightedList)
        {
            cumulative += weight;
            if (roll <= cumulative)
                return shape;
        }

        // Fallback in case of rounding error
        return allShapes[Random.Range(0, allShapes.Count)];
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


    private ShapeItem GetRandomShapeByRarity(List<ShapeItem> allShapes)
    {
        Dictionary<Rarity, float> rarityWeights = new()
    {
        { Rarity.Common, 50f },
        { Rarity.Rare, 30f },
        { Rarity.Epic, 15f },
        { Rarity.Legendary, 4f },
        { Rarity.Mythic, 1f }
    };

        // Step 1: Roll for rarity
        float totalWeight = rarityWeights.Values.Sum();
        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        Rarity chosenRarity = Rarity.Common;

        foreach (var kvp in rarityWeights)
        {
            cumulative += kvp.Value;
            if (roll <= cumulative)
            {
                chosenRarity = kvp.Key;
                break;
            }
        }

        // Step 2: Filter shapes of chosen rarity
        var matchingShapes = allShapes.Where(s => s.rarity == chosenRarity).ToList();

        if (matchingShapes.Count == 0)
        {
            // fallback: pick any shape at all
            return allShapes[Random.Range(0, allShapes.Count)];
        }

        // Step 3: Random pick from matching
        return matchingShapes[Random.Range(0, matchingShapes.Count)];
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