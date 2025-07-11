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

    private Queue<Action> rewardQueue = new Queue<Action>();
    private bool isShowing = false;

    void Start()
    {
        popupPanel.SetActive(false);
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

    private void ShowDuplicateRefundInternal(float refundAmount, ShapeItem shape)
    {
        gameObject.SetActive(true);
        popupPanel.SetActive(true);
        shapeImage.gameObject.SetActive(true);

        shapeImage.sprite = shape.icon;
        shapeImage.color = shape.GetRarityColor();
        shapeText.text = $"Duplicate {shape.id}! Refunded {refundAmount} coins (¼ of price)";
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
            var action = rewardQueue.Dequeue();
            action.Invoke();
            isShowing = true;
        }
        else
        {
            Hide();
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
}
