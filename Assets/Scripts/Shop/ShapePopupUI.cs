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
        shapeImage.sprite = icon;
        shapeText.text = shapeName;
    }


    public void Hide()
    {
        popupPanel.SetActive(false);
    }
}
