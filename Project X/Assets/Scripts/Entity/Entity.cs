using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List <EntityTag> TagMask = new();
    private Resource[] _resourcesGained;

    private void Awake()
    {
        InitializeEntityResources();
    }

    private void InitializeEntityResources()
    {
        _resourcesGained = new Resource[2];
        _resourcesGained[0] = new Resource { Name = "Fame", Type = ResourceType.Fame, Quantity = 0 };
        _resourcesGained[1] = new Resource { Name = "Gold", Type = ResourceType.Gold, Quantity = 0 };
    }

    public void AddEntityResource(ResourceType type, int quantity)
    {
        foreach (var resource in _resourcesGained)
        {
            if (resource.Type == type)
            {
                resource.Quantity += quantity;
                Debug.Log($"Added {quantity} {type}. Total: {resource.Quantity}");
                return;
            }
        }
    }
    // Check if the entity has a specific tag
    public bool CheckEntityTag(EntityTag enemyEntityTag)
    {
        return TagMask.Contains(enemyEntityTag);
    }

    public bool CheckEntityTag(EntityTag[] enemyEntityTags)
    {
        foreach (EntityTag tag in enemyEntityTags)
        {
            if (TagMask.Contains(tag))
                return true;
        }
        return false;
    }

    public void Die()
    {
        // Entity death logic
    }
}
