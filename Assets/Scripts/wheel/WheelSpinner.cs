using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpinner : MonoBehaviour
{
    public Transform wheelTransform;         // Assign your wheel GameObject
    public Button spinButton;                // Assign in Inspector
    public float spinDuration = 3f;          // Duration of the spin
    public int minRotation = 3;              // Min full spins
    public int maxRotation = 6;              // Max full spins

    private bool isSpinning = false;

    void Start()
    {
        spinButton.onClick.AddListener(StartSpin);
    }

    public void StartSpin()
    {
        if (isSpinning) return;

        StartCoroutine(SpinWheel());
    }

    private IEnumerator SpinWheel()
    {
        isSpinning = true;
        spinButton.interactable = false;

        float totalAngle = Random.Range(minRotation, maxRotation) * 360f + Random.Range(0f, 360f);
        float currentAngle = 0f;
        float elapsed = 0f;

        float startAngle = wheelTransform.eulerAngles.z;
        float targetAngle = startAngle + totalAngle;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            float easedT = EaseOutCubic(t);
            float angle = Mathf.Lerp(startAngle, targetAngle, easedT);
            wheelTransform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }

        // Snap to final angle
        float finalZ = wheelTransform.eulerAngles.z % 360f;
        wheelTransform.eulerAngles = new Vector3(0, 0, finalZ);

        DeterminePrize(360f - finalZ); // Counter-clockwise correction
        spinButton.interactable = true;
        isSpinning = false;
    }

    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    private void DeterminePrize(float angle)
    {
        Debug.Log("Wheel stopped at angle: " + angle);

        // Define prize sectors (counter-clockwise, starting from 0° at top)
        if (angle >= 0f && angle < 45f)
        {
            Debug.Log("🎁 Prize 1: Double your coins!");
            ShapeManager.Instance.coinCount *= 2;
            // Optionally update UI
        }
        else if (angle >= 45f && angle < 90f)
        {
            Debug.Log("🎁 Prize 2: +100 Premium Coins!");
            ShapeManager.Instance.AddPremiumCoins(100);
        }
        else if (angle >= 90f && angle < 135f)
        {
            Debug.Log("🎁 Prize 3...");
        }
        // Add more ranges as needed (8 slices = 45 degrees per slice)
    }
}
