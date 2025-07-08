using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShapePopupUI : MonoBehaviour
{
    public GameObject popupPanel;      // The root panel (likely "ShapePopupPanel" itself)
    public Image shapeImage;
    public TMP_Text shapeText;

    void Start()
    {
        popupPanel.SetActive(false);   // Hide it on start
    }

    public void Show(Sprite icon, string shapeName)
    {
        Debug.Log("📦 Showing shape popup: " + shapeName);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(true); // 👈 make sure it's visible
        shapeImage.sprite = icon;
        shapeText.text = shapeName;
    }
    public void ShowDuplicateRefund(float refundAmount)
    {
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(false);
        shapeText.text = $"Duplicate! Refunded {refundAmount} coins (¼ of price)";
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
        shapeImage.gameObject.SetActive(true); // 👈 resets for next use
    }
    public void ShowMessage(string message)
    {
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(false);  // Hide image
        shapeText.text = message;
    }

}
