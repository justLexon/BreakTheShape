using UnityEngine;
using TMPro;

public class MaterialUpgradeButton : MonoBehaviour
{
    public TMP_Text CostText;
    public TMP_Text LevelCountText;

    public string materialKey = "Dirt";
    public int maxLevel = 10;

    private int level = 0;
    private double currentCost = 0;

    public static int upgradeAmount = 1; // 👈 Set externally by buttons

    void Start()
    {
        UpdateUI();
    }

    public void OnUpgradeClick()
    {
        level = MaterialsManager.Instance.GetMaterialLevel(materialKey);

        if (!MaterialsManager.Instance.CanUpgrade(materialKey, maxLevel))
            return;

        int targetAmount = upgradeAmount == -1 ? CalculateMaxAffordableUpgrades() : upgradeAmount;
        targetAmount = Mathf.Min(targetAmount, maxLevel - level);

        double totalCost = GetTotalUpgradeCost(level, targetAmount);

        if (ShapeManager.Instance.GetCoinCount() >= totalCost)
        {
            ShapeManager.Instance.SpendCoins(totalCost);

            for (int i = 0; i < targetAmount; i++)
            {
                MaterialsManager.Instance.UpgradeMaterial(materialKey);
            }

            level += targetAmount;

            int unlockedIndex = MaterialsManager.Instance.GetMaterialIndex(materialKey);
            if (unlockedIndex > MaterialsManager.Instance.GetCurrentMaterialIndex())
            {
                MaterialsManager.Instance.SetCurrentMaterial(unlockedIndex);
            }

            int health = MaterialsManager.Instance.GetUpgradePower(materialKey);
            int coinUpgrade = MaterialsManager.Instance.GetCoinUpgrade(materialKey);

            ShapeManager.Instance.baseMaxHealth += health * targetAmount;
            ShapeManager.Instance.coinsPerBreak += coinUpgrade * targetAmount;

            BuySound.Instance.PlayBuySound();
            UpdateUI();
            ShapeManager.Instance.LoadShapeFromSave(ShapeManager.Instance.GetCurrentShapeIndex());
        }
        else
        {
            Debug.Log("Not enough coins to upgrade material.");
        }
    }

    int CalculateMaxAffordableUpgrades()
    {
        double coins = ShapeManager.Instance.GetCoinCount();
        int upgrades = 0;
        double totalCost = 0;

        for (int i = level; i < maxLevel; i++)
        {
            double cost = MaterialsManager.Instance.GetUpgradeCostAtLevel(materialKey, i);
            totalCost += cost;
            if (totalCost > coins)
                break;
            upgrades++;
        }

        return upgrades;
    }

    double GetTotalUpgradeCost(int startLevel, int quantity)
    {
        double total = 0;
        for (int i = 0; i < quantity; i++)
        {
            total += MaterialsManager.Instance.GetUpgradeCostAtLevel(materialKey, startLevel + i);
        }
        return total;
    }

    void UpdateUI()
    {
        level = MaterialsManager.Instance.GetMaterialLevel(materialKey);
        currentCost = MaterialsManager.Instance.GetUpgradeCost(materialKey);

        CostText.text = FormatNumberWithSuffix(currentCost);
        LevelCountText.text = level.ToString();
    }

    string FormatNumberWithSuffix(double number)
    {
        if (number < 1000)
            return System.Math.Round(number).ToString();

        string[] suffixes = { "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
        int suffixIndex = -1;
        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }
        return number.ToString("0.#") + suffixes[suffixIndex];
    }
}
