using UnityEngine;
using TMPro;

public class DPSUpgradeButton : MonoBehaviour
{
    public TMP_Text CostText;
    public TMP_Text LevelCountText;

    public string upgradeKey = "Fist";
    public double baseCost = 50;
    public float costMultiplier = 1.15f;
    public int maxLevel = 999;
    public int upgradePower = 1;

    private int level = 0;
    private double currentCost;

    public static int upgradeAmount = 1; // 👈 controlled by external buttons

    void Start()
    {
        switch (upgradeKey)
        {
            case "Slingshot": baseCost = 50; upgradePower = 3; costMultiplier = 1.25f; break; // Dirt and Wood
            case "BowArrow": baseCost = 5000; upgradePower = 50; costMultiplier = 1.25f; break; // Stone and Granite
            case "Catapult": baseCost = 50000; upgradePower = 750; costMultiplier = 1.26f; break; // Marble and Bronze
            case "Drill": baseCost = 1000000; upgradePower = 5000; costMultiplier = 1.25f; break; // Silver
            case "Saw": baseCost = 500000000; upgradePower = 15000; costMultiplier = 1.25f; break; // Gold
            case "Jackhammer": baseCost = 10000000000; upgradePower = 115000; costMultiplier = 1.1f; break; // Jade
            case "FlameThrower": baseCost = 100000000000; upgradePower = 500000; costMultiplier = 1.1f; break; // Diamond
            default: baseCost = 100; break;
        }

        level = PlayerPrefs.GetInt(upgradeKey + "_Level", 0);
        UpdateUI();
    }

    public void OnUpgradeClick()
    {
        if (level >= maxLevel) return;

        int targetAmount = upgradeAmount == -1 ? CalculateMaxAffordableUpgrades() : upgradeAmount;
        targetAmount = Mathf.Min(targetAmount, maxLevel - level);

        double totalCost = GetTotalCost(level, targetAmount);

        if (ShapeManager.Instance.GetCoinCount() >= totalCost)
        {
            ShapeManager.Instance.SpendCoins(totalCost);
            level += targetAmount;

            ShapeManager.Instance.idleDamagePerSecond += upgradePower * targetAmount;

            PlayerPrefs.SetInt(upgradeKey + "_Level", level);
            PlayerPrefs.Save();

            BuySound.Instance.PlayBuySound();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    }

    int CalculateMaxAffordableUpgrades()
    {
        double coins = ShapeManager.Instance.GetCoinCount();
        int upgrades = 0;
        double totalCost = 0;

        for (int i = level; i < maxLevel; i++)
        {
            totalCost += System.Math.Round(baseCost * System.Math.Pow(costMultiplier, i));
            if (totalCost > coins) break;
            upgrades++;
        }

        return upgrades;
    }

    double GetTotalCost(int startLevel, int quantity)
    {
        double total = 0;
        for (int i = 0; i < quantity; i++)
        {
            total += System.Math.Round(baseCost * System.Math.Pow(costMultiplier, startLevel + i));
        }
        return total;
    }

    void UpdateUI()
    {
        currentCost = System.Math.Round(baseCost * System.Math.Pow(costMultiplier, level));
        CostText.text = FormatNumberWithSuffix(currentCost);
        LevelCountText.text = level.ToString();
    }

    string FormatNumberWithSuffix(double number)
    {
        if (number < 1000) return System.Math.Round(number).ToString();

        string[] suffixes = { "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
        int index = -1;
        while (number >= 1000 && index < suffixes.Length - 1)
        {
            number /= 1000;
            index++;
        }

        return number.ToString("0.#") + suffixes[index];
    }
}
