using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [Header("Overlay Pages")]
    public GameObject mainMenu;

    void Start()
    {
        if (mainMenu != null)
            mainMenu.SetActive(false); // Start inactive
    }

    // Toggle overlay visibility
    public void ToggleMainMenuOverlay()
    {
        if (mainMenu == null) return;

        bool isActive = mainMenu.activeSelf;
        mainMenu.SetActive(!isActive); // Toggle state
    }
}
