using UnityEngine;

public class CurrencyPickerButton : MonoBehaviour
{
    [Header("Shape Coins UI")]
    public GameObject shapeCoins;
    public GameObject shapeCoinsOverlay;

    [Header("Premium Coins UI")]
    public GameObject premCoins;
    public GameObject premCoinsOverlay;

    void Start()
    {
        // Start with ShapeCoins active by default
        ShowShapeCoinsOverlay();
    }

    public void ShowShapeCoinsOverlay()
    {
        if (shapeCoinsOverlay.activeSelf && shapeCoins.activeSelf) return;

        shapeCoins.SetActive(true);
        shapeCoinsOverlay.SetActive(true);

        premCoins.SetActive(false);
        premCoinsOverlay.SetActive(false);
    }

    public void ShowPremiumCoinsOverlay()
    {
        if (premCoinsOverlay.activeSelf && premCoins.activeSelf) return;

        shapeCoins.SetActive(false);
        shapeCoinsOverlay.SetActive(false);

        premCoins.SetActive(true);
        premCoinsOverlay.SetActive(true);
    }
}
