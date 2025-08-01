﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShapeIconUI : MonoBehaviour
{
    public Image shapeImage;
    public Image BG;
    public TMP_Text shapeName;
    public GameObject lockOverlay;      // Shown if not owned
    public GameObject enabledOverlay;   // Green overlay if enabled

    private string shapeId;
    private bool isOwned;
    private bool isEnabled;

    public void Setup(ShapeItem shape, bool owned, bool enabled)
    {
        shapeId = shape.id;
        isOwned = owned;
        isEnabled = enabled;

        shapeImage.sprite = shape.icon;
        shapeImage.color = shape.GetRarityColor(); // ✅ tint based on rarity
        BG.color = shape.GetRarityBackgroundColor();
        shapeName.color = shape.GetRarityBackgroundColor();
        shapeName.text = shape.id;
        UpdateVisuals();
    }


    public void OnClick()
    {
        if (!isOwned)
            return;

        bool currentlyEnabled = InventoryManager.Instance.IsShapeEnabled(shapeId);

        if (currentlyEnabled)
        {
            InventoryManager.Instance.SetShapeEnabled(shapeId, false);
            ShapeManager.Instance.RefreshEnabledShapes();
        }
        else if (InventoryManager.Instance.GetEnabledShapes().Count < 10)
        {
            InventoryManager.Instance.SetShapeEnabled(shapeId, true);
            ShapeManager.Instance.RefreshEnabledShapes();
        }

        // Update local isEnabled flag from InventoryManager after toggling
        isEnabled = InventoryManager.Instance.IsShapeEnabled(shapeId);
        UpdateVisuals();

        InventoryView.Instance.UpdateCurrentShapeEnabledText();

        SaveSystem.Instance.SaveProgress();
    }





    public void UpdateVisual(bool enabled)
    {
        isEnabled = enabled;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        lockOverlay.SetActive(!isOwned);
        enabledOverlay.SetActive(isOwned && isEnabled);
    }
}
