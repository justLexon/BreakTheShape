using UnityEngine;

public class SoundActivateIcons : MonoBehaviour
{
    public GameObject object1;
    //public GameObject object2;

    //// Call this to activate both objects
    //public void ActivateBoth()
    //{
    //    if (object1 != null)
    //        object1.SetActive(true);

    //    if (object2 != null)
    //        object2.SetActive(true);
    //}

    //// Call this to deactivate both objects
    //public void DeactivateBoth()
    //{
    //    if (object1 != null)
    //        object1.SetActive(false);

    //    if (object2 != null)
    //        object2.SetActive(false);
    //}
    private void Start()
    {
        CheckSoundIcon();
    }

    void Update()
    {
        CheckSoundIcon();
    } 

    public void CheckSoundIcon()
    {
        // Check if sound is enabled and audio exists
        if (ShapeManager.Instance.IsSoundEnabled() == true)
        {
            object1.SetActive(false);
        }
        if (ShapeManager.Instance.IsSoundEnabled() == false)
        {
            object1.SetActive(true);
        }

    }
}
