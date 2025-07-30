using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    [System.Serializable]
    public class Mission
    {
        public string missionName;
        public int shapesRequired;
        public int coinReward;
        public GameObject unCompletedLogo;
        public GameObject completedLogo;
        public bool isCompleted;

        public string GetSaveKey()
        {
            return "Mission_" + missionName.Replace(" ", "_");
        }
    }

    [Header("Mission Configuration")]
    public List<Mission> missions = new List<Mission>();

    void Start()
    {
        LoadMissionStates();
        CheckAllMissions();
    }

    void Update()
    {
        // Optional: Check missions every frame or call this from elsewhere
        // CheckAllMissions();
        CheckAllMissions();
    }

    public void CheckAllMissions()
    {
        double currentShapeCount = ShapeManager.Instance.shapesBrokenCounter;

        foreach (Mission mission in missions)
        {
            if (!mission.isCompleted && currentShapeCount >= mission.shapesRequired)
            {
                CompleteMission(mission);
            }
            UpdateMissionUI(mission);
        }
    }

    private void CompleteMission(Mission mission)
    {
        mission.isCompleted = true;
        ShapeManager.Instance.AddPremiumCoins(mission.coinReward);
        SaveMissionState(mission);

        Debug.Log($"Mission '{mission.missionName}' completed! Rewarded {mission.coinReward} coins.");
    }

    private void UpdateMissionUI(Mission mission)
    {
        if (mission.unCompletedLogo != null)
            mission.unCompletedLogo.SetActive(!mission.isCompleted);

        if (mission.completedLogo != null)
            mission.completedLogo.SetActive(mission.isCompleted);
    }

    private void SaveMissionState(Mission mission)
    {
        PlayerPrefs.SetInt(mission.GetSaveKey(), mission.isCompleted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadMissionStates()
    {
        foreach (Mission mission in missions)
        {
            mission.isCompleted = PlayerPrefs.GetInt(mission.GetSaveKey(), 0) == 1;
        }
    }

    // Optional: Method to reset all missions (useful for testing)
    [ContextMenu("Reset All Missions")]
    public void ResetAllMissions()
    {
        foreach (Mission mission in missions)
        {
            mission.isCompleted = false;
            PlayerPrefs.DeleteKey(mission.GetSaveKey());
            UpdateMissionUI(mission);
        }
        PlayerPrefs.Save();
        Debug.Log("All missions reset!");
    }

    // Optional: Get mission progress for UI display
    public float GetMissionProgress(int missionIndex)
    {
        if (missionIndex >= 0 && missionIndex < missions.Count)
        {
            Mission mission = missions[missionIndex];
            if (mission.isCompleted) return 1f;

            float progress = (float)ShapeManager.Instance.shapesBrokenCounter / mission.shapesRequired;
            return Mathf.Clamp01(progress);
        }
        return 0f;
    }
}