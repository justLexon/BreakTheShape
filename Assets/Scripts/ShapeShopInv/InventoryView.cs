using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    private Coroutine hideWarningCoroutine;
    public static InventoryView Instance;
    public Transform packContainer; // Content container under ScrollView
    public GameObject packUIPrefab; // Prefab for a row (ShapePackUI)
    public ScrollRect verticalScrollRect; // assign in inspector
    public GameObject inventoryOverlayPanel; // Assign the inventory root panel here
    public TMP_Text currentShapeEnabledText;
    public GameObject warningTextObj;
    public TMP_Text warningText;


    private void Awake()
    {
        Instance = this;
        warningTextObj.SetActive(false);
    }

    public void PopulateFromShop()
    {
        Populate(ShopManager.Instance.allShapePacks);
    }

    public void Populate(List<ShapePack> allPacks)
    {
        foreach (Transform child in packContainer)
            Destroy(child.gameObject);

        foreach (var pack in allPacks)
        {
            GameObject packGO = Instantiate(packUIPrefab, packContainer);
            ShapePackUI packUI = packGO.GetComponent<ShapePackUI>();

            NestedScrollRectHandler nestedHandler = packGO.GetComponent<NestedScrollRectHandler>();
            if (nestedHandler != null)
            {
                nestedHandler.SetParentScrollRect(verticalScrollRect);
            }

            packUI.Setup(pack);
        }
    }

    // Call this method from your close button OnClick event
    private Coroutine fadeCoroutine;

    public void TryCloseInventory()
    {
        int enabledCount = InventoryManager.Instance.GetEnabledShapes().Count;

        if (enabledCount != 10)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeWarning($"Enable 10 shapes before closing the inventory. Currently enabled: {enabledCount}", 2f));
            return;
        }

        // Hide instantly if everything is valid
        warningTextObj.SetActive(false);
        inventoryOverlayPanel.SetActive(false);
        ShapeManager.Instance.RefreshEnabledShapes();
    }

    private IEnumerator FadeWarning(string message, float duration)
    {
        CanvasGroup canvasGroup = warningTextObj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = warningTextObj.AddComponent<CanvasGroup>();

        warningText.text = message;
        warningTextObj.SetActive(true);

        // Fade In
        canvasGroup.alpha = 0f;
        float fadeTime = 0.5f;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Wait
        yield return new WaitForSeconds(duration);

        // Fade Out
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        warningTextObj.SetActive(false);
    }



    private IEnumerator HideWarningAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        warningTextObj.SetActive(false);
    }


    public void UpdateCurrentShapeEnabledText()
    {
        int enabledCount = InventoryManager.Instance.GetEnabledShapes().Count;
        currentShapeEnabledText.text = $"{enabledCount}/10";
    }

}
