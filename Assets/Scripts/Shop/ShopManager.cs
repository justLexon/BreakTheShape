using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public List<ShapePack> allShapePacks; // Assign all your packs in Inspector

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetPackCostsToDefault()
    {
        foreach (var pack in allShapePacks)
        {
            pack.cost = 100; // or whatever default you want
        }
    }
}
