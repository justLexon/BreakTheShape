using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class IdleRewardManager : MonoBehaviour
{
    private const string LastOnlineKey = "LastOnlineTime";
    public double idleRewardMultiplier = 0.25; // 25% of normal DPS income
    public float maxOfflineSeconds = 8 * 60 * 60; // 8 hours
    public TMP_Text idleRewardText;
    public GameObject idleRewardPanel;

    private void Start()
    {
        StartCoroutine(WaitForShapeManagerThenCheck());
    }

    private IEnumerator WaitForShapeManagerThenCheck()
    {
        // Wait until ShapeManager.Instance exists and is initialized
        while (ShapeManager.Instance == null || !ShapeManager.Instance.IsInitialized)
            yield return null;

        // Now safe to calculate with updated values
        CheckIdleReward();
    }

    void OnApplicationQuit()
    {
        SaveLastOnlineTime();
    }

    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
            SaveLastOnlineTime();
    }

    private void SaveLastOnlineTime()
    {
        PlayerPrefs.SetString(LastOnlineKey, DateTime.UtcNow.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public void ShowIdleRewardPopup(string reward, TimeSpan duration)
    {
        idleRewardText.text = $"{duration.Hours}h {duration.Minutes}m\n\n\n{reward} coins";
        idleRewardPanel.SetActive(true);
    }
    
    public void DisableOverlay()
    {
        idleRewardPanel.SetActive(false);
    }

    private void CheckIdleReward()
    {
        if (!PlayerPrefs.HasKey(LastOnlineKey))
            return;

        string binaryTime = PlayerPrefs.GetString(LastOnlineKey);
        DateTime lastTime = DateTime.FromBinary(Convert.ToInt64(binaryTime));
        DateTime now = DateTime.UtcNow;

        TimeSpan offlineTime = now - lastTime;
        double secondsOffline = Mathf.Min((float)offlineTime.TotalSeconds, maxOfflineSeconds);

        double idleDPS = ShapeManager.Instance.GetIdleDPS(); // You must implement this method

        //double reward = idleDPS * secondsOffline * idleRewardMultiplier;
        double coinsPerBreak = ShapeManager.Instance.coinsPerBreak;
        double totalOfflineDamage = idleDPS * secondsOffline;
        int shapesBroken = Mathf.FloorToInt((float)(totalOfflineDamage * 0.5 / ShapeManager.Instance.baseMaxHealth * idleRewardMultiplier));
        double reward = shapesBroken * coinsPerBreak;
        Debug.Log($"IdleReward Debug => secondsOffline: {secondsOffline}, idleDPS: {idleDPS}, totalDamage: {totalOfflineDamage}, shapeHP: {ShapeManager.Instance.baseMaxHealth}, shapesBroken: {shapesBroken}, coinsPerBreak: {coinsPerBreak}, reward: {reward}");


        if (reward > 0)
        {
            ShapeManager.Instance.coinCount += reward;
            ShapeManager.Instance.uiManager.UpdateCoinText(ShapeManager.Instance.coinCount);

            ShowIdleRewardPopup(FormatNumberWithSuffix(reward), offlineTime);
        }
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
