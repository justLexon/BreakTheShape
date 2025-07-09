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
            case "Slingshot": baseCost = 50; upgradePower = 1; break;
            case "BowArrow": baseCost = 250; upgradePower = 5; break;
            case "Catapult": baseCost = 500; upgradePower = 15; break;
            case "Drill": baseCost = 1000; upgradePower = 50; break;
            case "Saw": baseCost = 5000; upgradePower = 150; break;
            case "Jackhammer": baseCost = 50000; upgradePower = 250; break;
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
