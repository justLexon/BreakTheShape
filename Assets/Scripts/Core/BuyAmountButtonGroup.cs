using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuyAmountButtonGroup : MonoBehaviour
{
    public List<Button> buttons;
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.85f, 0.3f); // Example highlight (goldish)

    private Button selectedButton;

    private void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClicked(btn));
        }

        // Optionally highlight the first by default
        if (buttons.Count > 0)
            OnButtonClicked(buttons[0]);
    }

    private void OnButtonClicked(Button clickedButton)
    {
        selectedButton = clickedButton;
        UpdateButtonVisuals();

        // You can trigger your buy amount logic here too if needed
        // Example:
        // ShapePackBuyAmountSetter.BuyAmount = int.Parse(clickedButton.GetComponentInChildren<Text>().text.Replace("x", ""));
    }

    private void UpdateButtonVisuals()
    {
        foreach (Button btn in buttons)
        {
            var colors = btn.colors;
            colors.normalColor = (btn == selectedButton) ? selectedColor : normalColor;
            colors.highlightedColor = colors.normalColor; // Makes hover match
            btn.colors = colors;
        }
    }
}
