using UnityEngine;
using UnityEngine.UI;
using TMPro; // ✅ Required for TMP_Text

public class ShapePackUI : MonoBehaviour
{
    public Image packImage;
    public TMP_Text packNameText; // ✅ Add this
    public RectTransform contentParent;
    public GameObject shapeIconPrefab;

    private ShapePack shapePack;

    public void Setup(ShapePack pack)
    {
        shapePack = pack;

        // ✅ Force these to active just in case
        if (packImage != null)
            packImage.gameObject.SetActive(true);
        if (packNameText != null)
            packNameText.gameObject.SetActive(true);

        // ✅ Set visuals
        if (packImage != null && pack.packSprite != null)
            packImage.sprite = pack.packSprite;
        if (packNameText != null)
            packNameText.text = pack.packName;

        // ✅ Clear old icons
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // ✅ Add all shape icons
        foreach (ShapeItem shape in pack.shapes)
        {
            GameObject iconGO = Instantiate(shapeIconPrefab, contentParent);
            ShapeIconUI iconUI = iconGO.GetComponent<ShapeIconUI>();

            bool isOwned = SaveManager.Instance.IsShapeOwned(shape.id);
            bool isEnabled = InventoryManager.Instance.IsShapeEnabled(shape.id);

            iconUI.Setup(shape.id, shape.icon, isOwned, isEnabled);

            iconGO.GetComponent<Button>().onClick.AddListener(() =>
            {
                InventoryManager.Instance.ToggleShape(shape.id);
                iconUI.UpdateVisual(InventoryManager.Instance.IsShapeEnabled(shape.id));
            });
        }
    }
}
