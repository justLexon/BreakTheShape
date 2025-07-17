using System.Collections;
using UnityEngine;

public class ShapePulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseScale = 0.9f;
    public float pulseDuration = 0.1f;

    private Coroutine pulseCoroutine;
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Pulse()
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        // Always reset to original before starting
        transform.localScale = originalScale;
        pulseCoroutine = StartCoroutine(PulseEffect());
    }

    private IEnumerator PulseEffect()
    {
        Vector3 targetScale = originalScale * pulseScale;

        // Shrink
        float t = 0f;
        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float progress = t / pulseDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }

        // Grow back
        t = 0f;
        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float progress = t / pulseDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
        pulseCoroutine = null;
    }
}
