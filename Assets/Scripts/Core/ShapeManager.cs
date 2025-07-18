using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShapeSData
{
    public Sprite sprite;
}

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance { get; private set; }

    [Header("Universal Stats")]
    public double baseMaxHealth = 10;
    public double currentMaxHealth = 10;
    public double currentHealth = 10;
    public double tapDamage = 1;
    public double idleDamagePerSecond = 1f;
    public double coinsPerBreak = 5;
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

    [Header("Monies")]
    public double coinCount = 0;
    public double premiumCoinCount = 5555;

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

    public bool IsInitialized { get; private set; } = false;

    private void Start()
    {
        Debug.Log("🔁 ShapeManager Start — Loading Save");
        SaveSystem.Instance.LoadProgress();

        // Find polygon pack (by ID or name)
        ShapePack polygonPack = ShopManager.Instance.allShapePacks.Find(p => p.packId == "polygonPack"); // Replace with actual packId

        if (polygonPack != null)
        {
            InventoryManager.Instance.OwnAndEnableAllShapesInPack(polygonPack);
        }

        RefreshEnabledShapes(); // Populate shapes[] with enabled shapes including polygon pack

        IsInitialized = true;
    }


    private void InitializeDefaultShapes()
    {
        if (!PlayerPrefs.HasKey("HasInitializedDefaultShapes"))
        {
            Debug.Log("⚙️ Initializing default enabled shapes (Basic Polygon Pack)");

            // Find your basic polygon pack by ID or name
            ShapePack basicPack = null;
            foreach (var pack in ShopManager.Instance.allShapePacks)
            {
                if (pack.packId == "polygonPack") // Change to your actual packId or name
                {
                    basicPack = pack;
                    break;
                }
            }

            if (basicPack != null)
            {
                foreach (var shape in basicPack.shapes)
                {
                    InventoryManager.Instance.SetShapeEnabled(shape.id, true);
                    Debug.Log($"Enabled shape: {shape.id}");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Basic Polygon Pack not found!");
            }

            PlayerPrefs.SetInt("HasInitializedDefaultShapes", 1);
            PlayerPrefs.Save();
        }
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

    private void ApplyDamage(double damageAmount)
    {
        if (currentHealth <= 0) return;

        double damageDouble = damageAmount;

        if (currentHealth - damageDouble <= 0)
        {
            currentHealth = currentHealth > 1 ? 1f : 0f;
        }
        else
        {
            currentHealth -= damageDouble;
        }

        UpdateCrackVisual();

        if (currentHealth <= 0)
        {
            BreakShape();
        }
    }

    private void BreakShape()
    {
        if (shapes == null || shapes.Length == 0)
        {
            Debug.LogWarning("BreakShape called but shapes array is empty!");
            return;
        }

        coinCount += coinsPerBreak;
        shapesBrokenCounter++;

        uiManager.UpdateCoinText(coinCount);
        uiManager.UpdatePremiumCoinText(premiumCoinCount);
        uiManager.UpdateShapesBrokenText(shapesBrokenCounter);

        // Advance to the next shape and next material texture
        currentShapeIndex = (currentShapeIndex + 1) % shapes.Length;
        materialsManager.AdvanceTextureCycle(); // ✅ Advance material texture index

        LoadShape(currentShapeIndex);

        SaveSystem.Instance.SaveProgress();
    }

    public void LoadShapeFromSave(int index)
    {
        if (shapes == null || shapes.Length == 0)
        {
            Debug.LogWarning("LoadShapeFromSave called but shapes array is empty!");
            return;
        }

        currentShapeIndex = index % shapes.Length;
        LoadShape(currentShapeIndex);
        uiManager.UpdateCoinText(coinCount);
        uiManager.UpdatePremiumCoinText(premiumCoinCount);
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
            shadowRenderer.transform.localPosition = new Vector3(0f, -0.06f, 0.1f);
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
        double healthRatio = currentHealth / System.Math.Max(1.0, currentMaxHealth);
        double inverse = 1.0 - healthRatio;
        float eased = Mathf.Pow((float)inverse, 0.6f);
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

    public void AddCoins(double amount)
    {
        coinCount += amount;
        uiManager.UpdateCoinText(coinCount);
        SaveSystem.Instance.SaveProgress();
        Debug.Log($"💎 Added {amount} coins. Total: {coinCount}");
    }

    public double GetPremiumCoinCount()
    {
        return premiumCoinCount;
    }

    public void SpendPremiumCoins(double amount)
    {
        if (amount <= premiumCoinCount)
        {
            premiumCoinCount -= amount;
            uiManager.UpdatePremiumCoinText(premiumCoinCount); // You'll need to create this method in UIManagerS
            SaveSystem.Instance.SaveProgress();
            Debug.Log($"💎 Spent {amount} premium coins. Remaining: {premiumCoinCount}");
        }
        else
        {
            Debug.LogWarning("❌ Not enough premium coins to spend!");
        }
    }

    public void AddPremiumCoins(double amount)
    {
        premiumCoinCount += amount;
        uiManager.UpdatePremiumCoinText(premiumCoinCount);
        SaveSystem.Instance.SaveProgress();
        Debug.Log($"💎 Added {amount} premium coins. Total: {premiumCoinCount}");
    }


    public int GetCurrentShapeIndex() => currentShapeIndex;

    public void RefreshEnabledShapes()
    {
        var enabledIds = InventoryManager.Instance.GetEnabledShapes();
        Debug.Log($"Refreshing enabled shapes, count: {enabledIds.Count}");
        var allShapeItems = new List<ShapeData>();

        foreach (var id in enabledIds)
        {
            bool found = false;
            foreach (ShapePack pack in ShopManager.Instance.allShapePacks)
            {
                foreach (ShapeItem shape in pack.shapes)
                {
                    if (shape.id == id)
                    {
                        Debug.Log($"Adding shape to array: {shape.id}");
                        allShapeItems.Add(new ShapeData { sprite = shape.icon });
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
        }

        shapes = allShapeItems.ToArray();
        currentShapeIndex = 0;
        LoadShapeFromSave(currentShapeIndex);
    }

    public double GetIdleDPS()
    {
        return idleDamagePerSecond;
    }

}
