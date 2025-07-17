using System;
using UnityEngine;
using TMPro;

public class SpinCooldownManager : MonoBehaviour
{
    public GameObject readyTextObj;
    public GameObject cooldownTextObj;
    public TMP_Text cooldownText;
    public float cooldownHours = 24f;

    private DateTime nextSpinTime;
    private bool wasReady = false; // Track state to avoid unnecessary updates
    private const string LastSpinKey = "LastSpinTime";

    void Start()
    {
        CheckSpinStatus();
        wasReady = IsReadyToSpin();
    }

    void Update()
    {
        bool currentlyReady = IsReadyToSpin();

        // If state changed from not ready to ready
        if (currentlyReady && !wasReady)
        {
            ShowReady();
        }
        // If state changed from ready to not ready
        else if (!currentlyReady && wasReady)
        {
            ShowCooldown();
        }

        // Update countdown text if not ready
        if (!currentlyReady)
        {
            TimeSpan remaining = nextSpinTime - DateTime.UtcNow;
            if (remaining.TotalSeconds <= 0)
            {
                // Force transition to ready state
                ShowReady();
                currentlyReady = true;
            }
            else
            {
                cooldownText.text = FormatTime(remaining);
            }
        }

        wasReady = currentlyReady;
    }

    private void CheckSpinStatus()
    {
        if (PlayerPrefs.HasKey(LastSpinKey))
        {
            string savedTime = PlayerPrefs.GetString(LastSpinKey);
            nextSpinTime = DateTime.Parse(savedTime).AddHours(cooldownHours);
        }
        else
        {
            nextSpinTime = DateTime.MinValue; // First time player
        }

        if (IsReadyToSpin())
            ShowReady();
        else
            ShowCooldown();
    }

    public bool IsReadyToSpin()
    {
        return DateTime.UtcNow >= nextSpinTime;
    }

    private void ShowReady()
    {
        readyTextObj.SetActive(true);
        cooldownTextObj.SetActive(false);
    }

    private void ShowCooldown()
    {
        readyTextObj.SetActive(false);
        cooldownTextObj.SetActive(true);
    }

    public void OnSpinUsed()
    {
        DateTime now = DateTime.UtcNow;
        PlayerPrefs.SetString(LastSpinKey, now.ToString());
        PlayerPrefs.Save();
        nextSpinTime = now.AddHours(cooldownHours);
        ShowCooldown();
        wasReady = false; // Update state tracker
    }

    private string FormatTime(TimeSpan time)
    {
        return string.Format("Next spin in: {0:D2}:{1:D2}:{2:D2}",
            time.Hours + time.Days * 24,
            time.Minutes,
            time.Seconds);
    }
}