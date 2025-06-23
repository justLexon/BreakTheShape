using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject upgrades;

    public void ToggleUpgradeOverlay()
    {
        bool isActive = upgrades.activeSelf;
        upgrades.SetActive(!isActive);
    }
}
