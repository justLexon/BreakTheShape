// MaterialManager.cs
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Sprite[] materialSprites;
    private int currentMaterialIndex = 0;

    public Sprite GetNextMaterial()
    {
        if (materialSprites.Length == 0) return null;
        currentMaterialIndex = (currentMaterialIndex + 1) % materialSprites.Length;
        return materialSprites[currentMaterialIndex];
    }
}