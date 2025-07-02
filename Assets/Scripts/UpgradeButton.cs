using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text CostText;
    public TMP_Text LevelCountText;

    public string upgradeKey = "Fist";      // Unique key to identify this upgrade (e.g. "Fist", "Bat")

    public int baseCost = 50;               // Base cost of the upgrade
    public float costMultiplier = 1.15f;    // Exponential cost growth
    public int maxLevel = 999;              // Max upgrade level
    public int upgradePower = 1;            // Tap damage increase per upgrade

    private int level = 1;
    private int currentCost;

    void Start()
    {
        // Load saved level
        level = PlayerPrefs.GetInt(upgradeKey + "_Level", 1);
        UpdateUI();
    }

    public void OnUpgradeClick()
    {
        if (level >= maxLevel) return;

        currentCost = Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level - 1));

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

    void UpdateUI()
    {
        currentCost = Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level - 1));
        CostText.text = currentCost.ToString();
        LevelCountText.text = level.ToString();
    }
}
