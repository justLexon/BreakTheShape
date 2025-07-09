using UnityEngine;

public class DPTUpgradeAmountSetter : MonoBehaviour
{
    public void SetUpgradeAmount(int amount)
    {
        DPTUpgradeButton.upgradeAmount = amount;
        Debug.Log("🟩 DPT Upgrade amount set to: " + (amount == -1 ? "MAX" : amount.ToString()));
    }
}
