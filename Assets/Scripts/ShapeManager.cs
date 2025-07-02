using UnityEngine;
using TMPro;

[System.Serializable]
public class ShapeData
{
    public Sprite sprite;
    public float crackSpeed = 0.0f; // Currently unused but you can apply it for unique easing later
}

public class ShapeManager : MonoBehaviour
{
    [Header("Universal Stats")]
    public int maxHealth = 10;
    public float currentHealth;
    public int tapDamage = 1;
    public float idleDamagePerSecond = 0.5f;
    public int coinsPerBreak = 5;
    public double shapesBrokenCounter = 0;

    [Header("Shape Setup")]
    public ShapeData[] shapes;

    [Header("Rendering")]
    public SpriteRenderer shapeRenderer;
    public Material sharedCrackMaterial;

    [Header("UI")]
    public TMP_Text coinText;
    public TMP_Text shapesBrokenText;

    private int currentShapeIndex = 0;
    public double coinCount = 0;
    private float idleTimer = 0f;

    private Material currentMaterialInstance;

    public static ShapeManager Instance { get; private set; }

    // -------------------- LIFECYCLE --------------------

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 🔄 Reset save if this is the first time launching the game
        if (!PlayerPrefs.HasKey("HasLaunchedBefore"))
        {
            PlayerPrefs.DeleteAll(); // Clear any old test data
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        SaveSystem.Instance.LoadProgress(); // ✅ Load saved values
    }

    void Update()
    {
        if (currentHealth > 0)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 1f)
            {
                ApplyDamage(idleDamagePerSecond);
                idleTimer = 0f;
            }
        }
    }

    // -------------------- MAIN ACTIONS --------------------

    public void OnTap()
    {
        ApplyDamage(tapDamage);
        SaveSystem.Instance.SaveProgress();
    }

    void ApplyDamage(float damageAmount)
    {
        if (currentHealth <= 0) return;

        int damageInt = Mathf.RoundToInt(damageAmount);

        // Clamp damage so shape always ends at 0 or 0.1f
        if (currentHealth - damageInt <= 0)
        {
            currentHealth = currentHealth > 1 ? 0.1f : 0f;
        }
        else
        {
            currentHealth -= damageInt;
        }

        UpdateCrackVisual();

        if (currentHealth <= 0)
        {
            BreakShape();
        }
    }

    void BreakShape()
    {
        coinCount += coinsPerBreak;
        shapesBrokenCounter++;

        UpdateCoinUI();
        UpdateShapesBrokenCounter();

        currentShapeIndex = (currentShapeIndex + 1) % shapes.Length;
        LoadShape(currentShapeIndex);

        SaveSystem.Instance.SaveProgress();
    }

    public void LoadShapeFromSave(int index)
    {
        currentShapeIndex = index % shapes.Length;
        LoadShape(currentShapeIndex);
        UpdateCoinUI();
        UpdateShapesBrokenCounter();
    }

    void LoadShape(int index)
    {
        ShapeData shape = shapes[index];
        currentHealth = maxHealth;
        idleTimer = 0f;

        shapeRenderer.sprite = shape.sprite;

        // Clone material to isolate cracks
        currentMaterialInstance = new Material(sharedCrackMaterial);
        shapeRenderer.material = currentMaterialInstance;
        currentMaterialInstance.SetFloat("_CrackAmount", 0.7f); // No cracks at start
    }

    // -------------------- UI --------------------

    void UpdateCrackVisual()
    {
        float healthRatio = currentHealth / maxHealth;
        float inverse = 1f - healthRatio;
        float eased = Mathf.Pow(inverse, 0.6f); // Adjust crack intensity curve
        float crackAmount = Mathf.Lerp(0.7f, 0f, eased);

        if (currentMaterialInstance != null)
        {
            currentMaterialInstance.SetFloat("_CrackAmount", crackAmount);
        }
    }

    void UpdateCoinUI()
    {
        coinText.text = FormatNumberWithSuffix((double)coinCount);
    }


    void UpdateShapesBrokenCounter()
    {
        shapesBrokenText.text = FormatNumberWithSuffix((double)shapesBrokenCounter);
    }

    // -------------------- Number Formatting --------------------

    string FormatNumberWithSuffix(double number)
    {
        if (number < 1000)
            return number.ToString();

        string[] suffixes = {
        "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
    };

        int suffixIndex = -1;
        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }

        return number.ToString("0.#") + suffixes[suffixIndex];
    }


    // -------------------- Public Access --------------------

    public double GetCoinCount() => coinCount;

    public void SpendCoins(double amount)
    {
        if (amount <= coinCount)
        {
            coinCount -= amount;
            UpdateCoinUI();
            SaveSystem.Instance.SaveProgress();
        }
        else
        {
            Debug.LogWarning("Not enough coins to spend!");
        }
    }

    public int GetCurrentShapeIndex() => currentShapeIndex;
}
