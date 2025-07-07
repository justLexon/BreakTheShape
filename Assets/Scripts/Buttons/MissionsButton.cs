using UnityEngine;

public class MissionsButton : MonoBehaviour
{
    [Header("Overlay Pages")]
    public GameObject missions;

    void Start()
    {
        if (missions != null)
            missions.SetActive(false); // Start inactive
    }

    // Toggle overlay visibility
    public void ToggleMissionsOverlay()
    {
        if (missions == null) return;

        bool isActive = missions.activeSelf;
        missions.SetActive(!isActive); // Toggle state
    }
}
