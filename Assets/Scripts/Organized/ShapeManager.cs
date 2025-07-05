using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShapeSData
{
    public Sprite sprite;
}

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance { get; private set; }

    [Header("Universal Stats")]
    public int baseMaxHealth = 10;
    private int currentMaxHealth;
    public float currentHealth;
    public int tapDamage = 1;
    public float idleDamagePerSecond = 0.5f;
    public int coinsPerBreak = 5;
    public double shapesBrokenCounter = 0;

    [Header("Shape Setup")]
    public ShapeData[] shapes;

    [Header("Rendering")]
    public SpriteRenderer shapeRenderer;
    public SpriteRenderer shadowRenderer;
    public Material sharedCrackMaterial;

    [Header("References")]
    public MaterialsManager materialsManager;
    public UIManagerS uiManager;

    private int currentShapeIndex = 0;
    private float idleTimer = 0f;
    private Material currentMaterialInstance;

    public double coinCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (!PlayerPrefs.HasKey("HasLaunchedBefore"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        Debug.Log("🔁 ShapeManager Start — Loading Save");
        SaveSystem.Instance.LoadProgress();
    }

    private void Update()
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

    public void OnTap()
    {
        ApplyDamage(tapDamage);
        SaveSystem.Instance.SaveProgress();
    }

    private void ApplyDamage(float damageAmount)
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

    private void BreakShape()
    {
        coinCount += coinsPerBreak;
        shapesBrokenCounter++;

        uiManager.UpdateCoinText(coinCount);
        uiManager.UpdateShapesBrokenText(shapesBrokenCounter);

        // Advance to the next shape and next material texture
        currentShapeIndex = (currentShapeIndex + 1) % shapes.Length;
        materialsManager.AdvanceTextureCycle(); // ✅ Advance material texture index

        LoadShape(currentShapeIndex);

        SaveSystem.Instance.SaveProgress();
    }


    public void LoadShapeFromSave(int index)
    {
        currentShapeIndex = index % shapes.Length;
        LoadShape(currentShapeIndex);
        uiManager.UpdateCoinText(coinCount);
        uiManager.UpdateShapesBrokenText(shapesBrokenCounter);
    }

    private void LoadShape(int index)
    {
        ShapeData shape = shapes[index];

        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;

        idleTimer = 0f;
        shapeRenderer.sprite = shape.sprite;

        if (shadowRenderer != null)
        {
            shadowRenderer.sprite = shape.sprite;
            shadowRenderer.color = new Color(0f, 0f, 0f, 1f);
            shadowRenderer.transform.localPosition = new Vector3(0f, -0.1f, 0.1f);
        }

        currentMaterialInstance = new Material(sharedCrackMaterial);
        currentMaterialInstance.SetTexture("_MainTex", shape.sprite.texture);

        // ✅ Get current material sprite
        Sprite matSprite = materialsManager.GetDisplayMaterialSprite();
        if (matSprite != null)
        {
            currentMaterialInstance.SetTexture("_OverlayTex", matSprite.texture);

            Rect texRect = matSprite.textureRect;
            Texture2D atlasTex = matSprite.texture;

            // ✅ Calculate UV rect correctly
            Vector4 uvRect = new Vector4(
                texRect.x / atlasTex.width,
                texRect.y / atlasTex.height,
                texRect.width / atlasTex.width,
                texRect.height / atlasTex.height
            );

            currentMaterialInstance.SetVector("_OverlayTex_UVRect", uvRect);
        }

        shapeRenderer.material = currentMaterialInstance;
        currentMaterialInstance.SetFloat("_CrackAmount", 0.7f);
    }


    private void UpdateCrackVisual()
    {
        float healthRatio = currentHealth / Mathf.Max(1f, currentMaxHealth);
        float inverse = 1f - healthRatio;
        float eased = Mathf.Pow(inverse, 0.6f);
        float crackAmount = Mathf.Lerp(0.7f, 0f, eased);

        if (currentMaterialInstance != null)
        {
            currentMaterialInstance.SetFloat("_CrackAmount", crackAmount);
        }
    }

    public double GetCoinCount()
    {
        return coinCount;
    }

    public void SpendCoins(double amount)
    {
        if (amount <= coinCount)
        {
            coinCount -= amount;
            uiManager.UpdateCoinText(coinCount);
            SaveSystem.Instance.SaveProgress();
            Debug.Log($"✅ Spent {amount} coins. Remaining: {coinCount}");
        }
        else
        {
            Debug.LogWarning("❌ Not enough coins to spend!");
        }
    }

    public int GetCurrentShapeIndex() => currentShapeIndex;
}
