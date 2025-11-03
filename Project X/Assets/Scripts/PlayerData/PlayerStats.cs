using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public static PlayerStats Instance { get; private set; } = new(); // Jachy Hu 30/10
    // now it's a plain singleton
    // class, in order to avoid creating
    // a playerStats manager

    [ShowInInspector, DisableInEditorMode] 
    private Resource[] _resources;
    [ShowInInspector, DisableInEditorMode] 
    private List<Purchasable> _purchasedItems = new();  
    [ShowInInspector, DisableInEditorMode] 
    private Inventory _playerInventory;

    private const int STARTING_GOLD = 100;
    private const int STARTING_FAME = 50;

    //public Purchasable[] PurchasableItems;
    private PlayerStats()
    {
        InitializeResources();
        InitializeInventory();
    }

    private void InitializeResources()
    {
        _resources = new Resource[2];
        _resources[0] = new Resource { Name = "Fame", Type = ResourceType.Fame, Quantity = STARTING_FAME };
        _resources[1] = new Resource { Name = "Gold", Type = ResourceType.Gold, Quantity = STARTING_GOLD };

        // when is ready do the same logic for PurchasableItems
        Debug.Log($"Resources initialized: {_resources[0].Name}({_resources[0].Quantity}), " +
                  $"{_resources[1].Name}({_resources[1].Quantity})");
    }

    private void InitializeInventory()
    {
        _playerInventory = new Inventory();
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

    public Inventory GetInventory()
    {
        return _playerInventory;
    }

    public void AddPurchasableItem(Purchasable purchasableItem)
    {
        _purchasedItems.Add(purchasableItem);
    }

    public int GetPurchasedItems(Purchasable purchasableItem)
    {
        return _purchasedItems.Count(p => p == purchasableItem);
    }
}