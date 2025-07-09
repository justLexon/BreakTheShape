using UnityEngine;

public class DPSUpgradeAmountSetter : MonoBehaviour
{
    public void SetUpgradeAmount(int amount)
    {
        DPSUpgradeButton.upgradeAmount = amount;
        Debug.Log("💡 Upgrade amount set to: " + (amount == -1 ? "MAX" : amount.ToString()));
    }
}
