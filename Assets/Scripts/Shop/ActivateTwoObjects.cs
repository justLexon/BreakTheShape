using UnityEngine;

public class ToggleTwoObjectsManual : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    // Call this to activate both objects
    public void ActivateBoth()
    {
        if (object1 != null)
            object1.SetActive(true);

        if (object2 != null)
            object2.SetActive(true);
    }

    // Call this to deactivate both objects
    public void DeactivateBoth()
    {
        if (object1 != null)
            object1.SetActive(false);

        if (object2 != null)
            object2.SetActive(false);
    }
}
