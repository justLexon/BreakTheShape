using TMPro;
using UnityEngine;

public class UIManagerS : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text shapesBrokenText;
    public TMP_Text ShopcoinText;
    public TMP_Text coinTextMat;
    public TMP_Text coinTextDPT;
    public TMP_Text coinTextDPS;


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
        coinTextMat.text = FormatNumberWithSuffix(coins);
        coinTextDPT.text = FormatNumberWithSuffix(coins);
        coinTextDPS.text = FormatNumberWithSuffix(coins);
    }

    public void UpdateShapesBrokenText(double shapesBroken)
    {
        shapesBrokenText.text = FormatNumberWithSuffix(shapesBroken);
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
