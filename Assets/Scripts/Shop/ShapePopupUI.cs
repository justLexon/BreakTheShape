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
        popupPanel.SetActive(true);
        popupPanel.SetActive(false);
    }

    public void Show(Sprite icon, string shapeName)
    {
        Debug.Log("📦 Showing shape popup: " + shapeName);
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(true);
        shapeImage.sprite = icon;
        shapeText.text = shapeName;
        Debug.Log("Popup visible: " + popupPanel.activeSelf);
    }

    public void ShowDuplicateRefund(float refundAmount)
    {
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(false);
        shapeText.text = $"Duplicate! Refunded {refundAmount} coins (¼ of price)";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        popupPanel.SetActive(false);
        shapeImage.gameObject.SetActive(true); // 👈 resets for next use
    }
    public void ShowMessage(string message)
    {
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(false);  // Hide image
        shapeText.text = message;
    }

}
