using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public static PlayerStats Instance { get; private set; } = new PlayerStats(); // Jachy Hu 30/10
                                                                                  // now it's a plain singleton
                                                                                  // class, in order to avoid creating
                                                                                  // a playerStats manager

    private Resource[] resources;
    //public Purchasable[] PurchasableItems;
    private PlayerStats()
    {
        initializeResources();
    }

    private void initializeResources()
    {
        resources = new Resource[2];
        resources[0] = new Resource { Name = "Fame", Type = ResourceType.Fame, Quantity = 0 };
        resources[1] = new Resource { Name = "Gold", Type = ResourceType.Gold, Quantity = 0 };
        
        // when is ready do the same logic for PurchasableItems
    }

    public void AddResource(ResourceType type, int quantity)
    {
        foreach (var resource in resources)
        {
            if (resource.Type == type)
            {
                resource.Quantity += quantity;
                return;
            }
        }
    }

    public int GetResourceQuantity(ResourceType type)
    {
        foreach (var resource in resources)
        {
            if (resource.Type == type)
            {
                return resource.Quantity;
            }
        }
        return 0;
    }
}