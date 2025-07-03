// UIManager.cs
using TMPro;
using UnityEngine;

public class UIManagerS : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text shapesBrokenText;

    public void UpdateCoinText(double coinCount)
    {
        coinText.text = FormatNumberWithSuffix(coinCount);
    }

    public void UpdateShapesBrokenText(double shapesBrokenCount)
    {
        shapesBrokenText.text = FormatNumberWithSuffix(shapesBrokenCount);
    }

    private string FormatNumberWithSuffix(double number)
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