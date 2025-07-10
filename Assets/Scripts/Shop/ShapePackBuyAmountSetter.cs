using UnityEngine;

public class ShapePackBuyAmountSetter : MonoBehaviour
{
    public static int BuyAmount { get; private set; } = 1;

    public void SetBuyAmount(int amount)
    {
        BuyAmount = amount;
        Debug.Log("💡 Upgrade amount set to: " + (BuyAmount.ToString()));
    }
}
