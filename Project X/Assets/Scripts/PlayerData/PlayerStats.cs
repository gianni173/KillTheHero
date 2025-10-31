using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public static PlayerStats Instance { get; private set; } = new(); // Jachy Hu 30/10
                                                                                  // now it's a plain singleton
                                                                                  // class, in order to avoid creating
                                                                                  // a playerStats manager

    private Resource[] _resources;
    private List<Purchasable> _purchasedItems = new();
    
    //public Purchasable[] PurchasableItems;
    private PlayerStats()
    {
        InitializeResources();
    }

    private void InitializeResources()
    {
        _resources = new Resource[2];
        _resources[0] = new Resource { Name = "Fame", Type = ResourceType.Fame, Quantity = 0 };
        _resources[1] = new Resource { Name = "Gold", Type = ResourceType.Gold, Quantity = 0 };
        
        // when is ready do the same logic for PurchasableItems
        Debug.Log($"Resources initialized: {_resources[0].Name}({_resources[0].Quantity}), {_resources[1].Name}({_resources[1].Quantity})");
    }

    public void AddResource(ResourceType type, int quantity)
    {
        foreach (var resource in _resources)
        {
            if (resource.Type == type)
            {
                resource.Quantity += quantity;
                Debug.Log($"Added {quantity} {type}. Total: {resource.Quantity}");
                return;
            }
        }
    }

    public int GetResourceQuantity(ResourceType type)
    {
        foreach (var resource in _resources)
        {
            if (resource.Type == type)
            {
                Debug.Log($"checking {type} amount. Total: {resource.Quantity}");
                return resource.Quantity;
            }
        }
        return 0;
    }

    public void AddPurchasableItem(Purchasable purchasableItem)
    {
        this._purchasedItems.Add(purchasableItem);
    }
}