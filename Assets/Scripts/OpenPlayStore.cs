using UnityEngine;

public class OpenPlayStore : MonoBehaviour
{
    public string playStoreURL = "https://play.google.com/store";

    public void OpenStorePage()
    {
        Application.OpenURL(playStoreURL);
    }
}
