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

    void Start()
    {
        UpdateUI();
    }

    public void OnUpgradeClick()
    {
        if (!MaterialsManager.Instance.CanUpgrade(materialKey, maxLevel))
            return;

        currentCost = MaterialsManager.Instance.GetUpgradeCost(materialKey);
        level = MaterialsManager.Instance.GetMaterialLevel(materialKey);

        if (ShapeManager.Instance.GetCoinCount() >= currentCost)
        {
            ShapeManager.Instance.SpendCoins(currentCost);

            MaterialsManager.Instance.UpgradeMaterial(materialKey);
            level++;

            // Apply upgrade power to gameplay stat, e.g. tapDamage
            int power = MaterialsManager.Instance.GetUpgradePower(materialKey);
            ShapeManager.Instance.tapDamage += power;

            UpdateUI();

            ShapeManager.Instance.LoadShapeFromSave(ShapeManager.Instance.GetCurrentShapeIndex());
        }
        else
        {
            Debug.Log("Not enough coins to upgrade material.");
        }
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
            return number.ToString("0");

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
