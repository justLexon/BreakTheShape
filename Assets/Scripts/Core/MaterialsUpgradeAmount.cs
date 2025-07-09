using UnityEngine;

public class MaterialUpgradeAmountSetter : MonoBehaviour
{
    public void SetUpgradeAmount(int amount)
    {
        MaterialUpgradeButton.upgradeAmount = amount;
        Debug.Log("🧱 Material Upgrade amount set to: " + (amount == -1 ? "MAX" : amount.ToString()));
    }
}
