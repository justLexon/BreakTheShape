using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WheelSpinner : MonoBehaviour
{
    public SpinCooldownManager cooldownManager;
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

    void Update()
    {
        // Update button interactability based on cooldown and spinning state
        UpdateButtonState();
    }

    public void StartSpin()
    {
        if (!cooldownManager.IsReadyToSpin()) return;
        if (isSpinning) return;

        StartCoroutine(SpinWheel());
        cooldownManager.OnSpinUsed();
    }

    private void UpdateButtonState()
    {
        // Button should only be interactable if ready to spin AND not currently spinning
        spinButton.interactable = cooldownManager.IsReadyToSpin() && !isSpinning;
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
            Debug.Log("🎁 Prize 8: 100 Premium coins!");
            ShapeManager.Instance.AddPremiumCoins(100);
        }
        else if (angle >= 45f && angle < 90f)
        {
            Debug.Log("🎁 Prize 7: Nothing");
        }
        else if (angle >= 90f && angle < 135f)
        {
            Debug.Log("🎁 Prize 6: 100k Coins!");
            ShapeManager.Instance.AddCoins(100000);
        }
        else if (angle >= 135f && angle < 180f)
        {
            Debug.Log("🎁 Prize 5: Triple your coins!");
            ShapeManager.Instance.AddCoins(ShapeManager.Instance.coinCount + ShapeManager.Instance.coinCount);
        }
        else if (angle >= 180f && angle < 225f)
        {
            Debug.Log("🎁 Prize 4: +200 Premium Coins!");
            ShapeManager.Instance.AddPremiumCoins(200);
        }
        else if (angle >= 225f && angle < 270f)
        {
            Debug.Log("🎁 Prize 3: Nothing");
        }
        else if (angle >= 270f && angle < 315f)
        {
            Debug.Log("🎁 Prize 2: Nothing");
        }
        else if (angle >= 315f && angle < 360f)
        {
            Debug.Log("🎁 Prize 1: Double your coins!");
            ShapeManager.Instance.AddCoins(ShapeManager.Instance.coinCount);
        }
    }
}