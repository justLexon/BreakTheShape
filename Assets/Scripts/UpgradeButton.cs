using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text CostText;
    public TMP_Text LevelCountText;

    public string upgradeKey = "Fist";      // Unique key to identify this upgrade (e.g. "Fist", "Bat")

    public double baseCost = 50;               // Base cost of the upgrade
    public float costMultiplier = 1.15f;    // Exponential cost growth
    public int maxLevel = 999;              // Max upgrade level
    public int upgradePower = 1;            // Tap damage increase per upgrade

    private int level = 0;
    private double currentCost;

    void Start()
    {
        // Load saved level (default 0)
        level = PlayerPrefs.GetInt(upgradeKey + "_Level", 0);
        UpdateUI();
    }

    public void OnUpgradeClick()
    {
        if (level >= maxLevel) return;

        currentCost = System.Math.Round(baseCost * System.Math.Pow(costMultiplier, level));

        if (ShapeManager.Instance.GetCoinCount() >= currentCost)
        {
            ShapeManager.Instance.SpendCoins(currentCost);
            level++;

            // Increase tap damage or stat
            ShapeManager.Instance.tapDamage += upgradePower;

            // Save the level
            PlayerPrefs.SetInt(upgradeKey + "_Level", level);
            PlayerPrefs.Save();

            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    }

    string FormatNumberWithSuffix(double number)
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

    void UpdateUI()
    {
        currentCost = System.Math.Round(baseCost * System.Math.Pow(costMultiplier, level));
        CostText.text = FormatNumberWithSuffix((double)currentCost);
        LevelCountText.text = level.ToString();
    }
}
