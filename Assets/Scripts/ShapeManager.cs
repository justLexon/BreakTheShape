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

    [Header("Material Sprites")]
    public Sprite[] materialSprites;  // Sprites sliced from your atlas for materials (dirt, wood, etc.)

    [Header("Rendering")]
    public SpriteRenderer shapeRenderer;
    public SpriteRenderer shadowRenderer; // Shadow renderer
    public Material sharedCrackMaterial;

    [Header("UI")]
    public TMP_Text coinText;
    public TMP_Text shapesBrokenText;

    private int currentShapeIndex = 0;
    private int currentMaterialIndex = 0;  // Tracks current material sprite index

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

        // Reset save if first launch
        if (!PlayerPrefs.HasKey("HasLaunchedBefore"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        SaveSystem.Instance.LoadProgress();
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
        currentMaterialIndex = (currentMaterialIndex + 1) % materialSprites.Length;

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

        // Set the main shape sprite
        shapeRenderer.sprite = shape.sprite;

        // Shadow
        if (shadowRenderer != null)
        {
            shadowRenderer.sprite = shape.sprite;
            shadowRenderer.color = new Color(0f, 0f, 0f, 0.3f);
            shadowRenderer.transform.localPosition = new Vector3(0f, -0.1f, 0.1f);
        }

        // Clone material to isolate cracks
        currentMaterialInstance = new Material(sharedCrackMaterial);

        // Set the main texture from the shape sprite texture
        currentMaterialInstance.SetTexture("_MainTex", shape.sprite.texture);

        // Set the overlay texture from the materialSprites array at current index
        Sprite currentMatSprite = materialSprites[currentMaterialIndex];
        currentMaterialInstance.SetTexture("_OverlayTex", currentMatSprite.texture);

        // Calculate UV rect for overlay sprite within atlas
        Vector4 uvRect = new Vector4(
            currentMatSprite.textureRect.x / currentMatSprite.texture.width,
            currentMatSprite.textureRect.y / currentMatSprite.texture.height,
            currentMatSprite.textureRect.width / currentMatSprite.texture.width,
            currentMatSprite.textureRect.height / currentMatSprite.texture.height
        );

        // Pass UV rect to shader (make sure your shader uses this)
        currentMaterialInstance.SetVector("_OverlayTex_UVRect", uvRect);

        shapeRenderer.material = currentMaterialInstance;

        currentMaterialInstance.SetFloat("_CrackAmount", 0.7f); // Start with no cracks
    }


    // -------------------- UI --------------------

    void UpdateCrackVisual()
    {
        float healthRatio = currentHealth / maxHealth;
        float inverse = 1f - healthRatio;
        float eased = Mathf.Pow(inverse, 0.6f);
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

        string[] suffixes = { "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };

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
