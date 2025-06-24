using UnityEngine;
using TMPro;
using JetBrains.Annotations;

[System.Serializable]

public class ShapeData
{
    public Sprite sprite;
    //public int maxHealth = 10;
    //public int tapDamage = 1;
    //public int coinsPerBreak = 5;
    //public float idleDamagePerSecond = 0.5f;  // New field for idle damage
    public float crackSpeed = 0.0f;
}

public class ShapeManager : MonoBehaviour
{
    [Header("Universal")]
    //Universal Damage for Every Shape
    public int maxHealth = 10;
    public float currentHealth;
    public int tapDamage = 1;
    public float idleDamagePerSecond = 0.5f;
    public int coinsPerBreak = 5;

    [Header("Shape Setup")]
    public ShapeData[] shapes;

    [Header("Rendering")]
    public SpriteRenderer shapeRenderer;
    public Material sharedCrackMaterial;

    [Header("UI")]
    public TMP_Text coinText;


    private int currentShapeIndex = 0;
    public int coinCount = 0;

    private float idleTimer = 0f;

    private Material currentMaterialInstance;

    void Start()
    {
        LoadShape(currentShapeIndex);
        UpdateCoinUI();
    }

    void Update()
    {
        if (currentHealth > 0)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 1f)
            {
                float idleDamage = idleDamagePerSecond;
                ApplyDamage(idleDamage);
                idleTimer = 0f;
            }
        }
    }

    public void OnTap()
    {
        ApplyDamage(tapDamage);
    }

    void ApplyDamage(float damageAmount)
    {
        if (currentHealth <= 0) return;

        int damageInt = Mathf.RoundToInt(damageAmount);

        if (currentHealth - damageInt <= 0)
        {
            // Prevent breaking on first tap
            if (currentHealth > 1)
            {
                currentHealth = 0.1f;
            }
            else
            {
                currentHealth = 0;
            }
        }
        else
        {
            currentHealth -= damageInt;
        }

        float healthRatio = (float)currentHealth / maxHealth;
        float inverse = 1f - healthRatio;

        float eased = Mathf.Pow(inverse, 0.6f); // Lower = faster crack reveal at start
        float crackAmount = Mathf.Lerp(0.7f, 0f, eased);


        //appear smooth underneath this comment
        //float eased = Mathf.SmoothStep(0f, 1f, inverse);
        //float crackAmount = Mathf.Lerp(0.7f, 0f, eased);
        // crackAmount = Mathf.Round(crackAmount * 20f) / 20f; // Optional rounding

        if (currentMaterialInstance != null)
        {
            currentMaterialInstance.SetFloat("_CrackAmount", crackAmount);
        }

        if (currentHealth <= 0)
        {
            BreakShape();
        }
    }


    void BreakShape()
    {
        coinCount += coinsPerBreak;
        UpdateCoinUI();

        currentShapeIndex = (currentShapeIndex + 1) % shapes.Length;
        LoadShape(currentShapeIndex);
    }

    void LoadShape(int index)
    {
        ShapeData shape = shapes[index];
        currentHealth = maxHealth;

        shapeRenderer.sprite = shape.sprite;

        currentMaterialInstance = new Material(sharedCrackMaterial);
        shapeRenderer.material = currentMaterialInstance;

        currentMaterialInstance.SetFloat("_CrackAmount", 0.7f); // Start with no cracks

        idleTimer = 0f; // Reset timer on new shape
    }

    void UpdateCoinUI()
    {
        coinText.text = coinCount.ToString();
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void SpendCoins(int amount)
    {
        coinCount -= amount;
        UpdateCoinUI();
    }

    // Optional: expose current shape index
    public int GetCurrentShapeIndex()
    {
        return currentShapeIndex;
    }

}
