using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    static public Action<Entity> OnDeath;
    public List <EntityTag> TagMask = new();
    private Resource[] _resourcesGained;
    private bool _hasReachedMimik = false;

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
                Debug.Log($"Added Entity {quantity} {type}. Total: {resource.Quantity}");
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
    //Jachy Hu 31/10: TODO add a logic when gaining a new tag? 
    //i don't know TagMask is public, maybe directly on AItem interact()?
    public void Die()
    {
        if (_hasReachedMimik)
        {
            var GoldGained = _resourcesGained[1].Quantity;
            PlayerStats.Instance.AddResource(ResourceType.Gold, GoldGained);
        }
        else
        {
            var FameGained =
                _resourcesGained[0].Quantity; // Jachy Hu 31/10: Horrendous, you need to know which slot in [] 
            // contains the specific type of resource, need to fix later
            PlayerStats.Instance.AddResource(ResourceType.Fame, FameGained);
        }
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
}
