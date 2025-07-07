using TMPro;
using UnityEngine;

public class UIManagerS : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text shapesBrokenText;
    public TMP_Text ShopcoinText;

    private ShapeManager shapeManager;

    void Start()
    {
        shapeManager = ShapeManager.Instance;
        UpdateCoinText(shapeManager.GetCoinCount());
        UpdateShapesBrokenText(shapeManager.shapesBrokenCounter);
    }

    public void UpdateCoinText(double coins)
    {
        coinText.text = FormatNumberWithSuffix(coins) + "+";
        ShopcoinText.text = FormatNumberWithSuffix(coins);
    }

    public void UpdateShapesBrokenText(double shapesBroken)
    {
        shapesBrokenText.text = FormatNumberWithSuffix(shapesBroken);
    }

    string FormatNumberWithSuffix(double number)
    {
        if (number < 1000)
            return number.ToString();

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
