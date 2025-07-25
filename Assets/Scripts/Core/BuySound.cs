using UnityEngine;

public class BuySound : MonoBehaviour
{
    public static BuySound Instance { get; private set; }

    public AudioSource audio;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Get or create AudioSource
        audio = GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
            Debug.Log("🎵 AudioSource component added to BuySound");
        }

        // Configure audio source
        audio.loop = false;
        audio.playOnAwake = false;
        audio.spatialBlend = 0f; // 2D sound
    }

    public void PlayBuySound()
    {
        // Check if sound is enabled and audio exists
        if (ShapeManager.Instance.IsSoundEnabled() && audio != null && audio.clip != null)
        {
            audio.Play();
            Debug.Log("🛒 Buy sound played");
        }
        else
        {
            if (!ShapeManager.Instance.IsSoundEnabled())
                Debug.Log("🔇 Buy sound disabled by user");
            if (audio == null)
                Debug.LogWarning("⚠️ AudioSource is null!");
            if (audio != null && audio.clip == null)
                Debug.LogWarning("⚠️ No AudioClip assigned to BuySound!");
        }
    }
}