using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ShapePopupUI : MonoBehaviour
{
    public GameObject popupPanel;
    public Image shapeImage;
    public TMP_Text shapeText;
    public TMP_Text packCounterText;

    private int currentPackIndex = 0;
    private int totalPacksToShow = 1; // default 1 to avoid division by 0



    private Queue<Action> rewardQueue = new Queue<Action>();
    private bool isShowing = false;

    void Start()
    {
        popupPanel.SetActive(false);
    }

    public void SetPackCounter(int totalPacks)
    {
        totalPacksToShow = Mathf.Max(totalPacks, 1); // Avoid 0
        currentPackIndex = 0;
    }

    // Public method to enqueue a reward
    public void EnqueueReward(ShapeItem shape)
    {
        rewardQueue.Enqueue(() => ShowInternal(shape));
    }


    // Public method to enqueue a duplicate refund
    public void EnqueueRefund(float refundAmount)
    {
        rewardQueue.Enqueue(() => ShowDuplicateRefundInternal(refundAmount));
    }

    public void EnqueueRefund(float amount, string customMessage)
    {
        rewardQueue.Enqueue(() =>
        {
            gameObject.SetActive(true);
            popupPanel.SetActive(true);
            shapeImage.gameObject.SetActive(false);
            shapeText.text = customMessage;
        });
    }

    public void EnqueueRefund(float refundAmount, ShapeItem shape)
    {
        rewardQueue.Enqueue(() => ShowDuplicateRefundInternal(refundAmount, shape));
    }

    private void ShowDuplicateRefundInternal(double refundAmount, ShapeItem shape)
    {
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(true);

        shapeImage.sprite = shape.icon;
        shapeImage.color = shape.GetRarityColor();
        shapeText.text = $"Duplicate {shape.id}!\nRefunded {FormatNumberWithSuffix(refundAmount)} coins";
    }



    // Show a message (used if all shapes owned, etc)
    public void ShowMessage(string message)
    {
        popupPanel.SetActive(true);
        gameObject.SetActive(true);
        shapeImage.gameObject.SetActive(false);
        shapeText.text = message;
        isShowing = true;
    }

    // Call this after purchases to show first item
    public void ShowNextInQueue()
    {
        if (rewardQueue.Count > 0)
        {
            currentPackIndex++;
            UpdatePackCounterText();

            var action = rewardQueue.Dequeue();
            action.Invoke();
            isShowing = true;
        }
        else
        {
            Hide();
        }
    }

    private void UpdatePackCounterText()
    {
        if (packCounterText != null)
        {
            packCounterText.gameObject.SetActive(true);
            packCounterText.text = $"{currentPackIndex}/{totalPacksToShow}";
        }
    }


    // Called from UI button click (e.g. popup background)
    public void OnPopupClicked()
    {
        ShowNextInQueue();
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
        shapeImage.gameObject.SetActive(true);
        isShowing = false;
    }

    // Internal method to show a shape reward
    private void ShowInternal(ShapeItem shape)
    {
        Debug.Log("📦 Showing shape popup: " + shape.id);
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(true);

        shapeImage.sprite = shape.icon;
        shapeImage.color = shape.GetRarityColor(); // ✅ set image color based on rarity
        shapeText.text = shape.id;
    }


    // Internal method to show refund message
    private void ShowDuplicateRefundInternal(float refundAmount)
    {
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(true);
        shapeText.text = $"Duplicate! Refunded {refundAmount} coins (¼ of price)";
    }

    private string FormatNumberWithSuffix(double number)
    {
        if (number < 1000)
            return number.ToString("0");

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
