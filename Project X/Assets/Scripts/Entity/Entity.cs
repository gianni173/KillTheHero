using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    static public Action<Entity> OnDeath;
    public List <EntityTag> TagMask = new();
    private Dictionary<ResourceType,Resource> _resourcesGained = new ();
    private bool _hasReachedMimik = false;

    [SerializeField]
    private Vector2Int _tagRange = Vector2Int.zero;
    [SerializeField]
    private Vector2Int _goldRange = Vector2Int.zero;
    [SerializeField]
    private Vector2Int _fameRange = Vector2Int.zero;

    private void Awake()
    {
        InitializeEntityResources();
        InitTags();
    }

    private void InitializeEntityResources()
    {
        _resourcesGained.Add(ResourceType.Fame, new Resource { Name = "Fame", Type = ResourceType.Fame, Quantity = UnityEngine.Random.Range(_fameRange.x, _fameRange.y + 1) });
        _resourcesGained.Add(ResourceType.Gold, new Resource { Name = "Gold", Type = ResourceType.Gold, Quantity = UnityEngine.Random.Range(_goldRange.x, _goldRange.y + 1) });
    }

    private void InitTags()
    {
        //get all hero tags
        var Tags = new List<EntityTag>(Enum.GetValues(typeof(EntityTag)) as EntityTag[]);
        int tagNumber = UnityEngine.Random.Range(_tagRange.x, _tagRange.y + 1);
        TagMask.Clear();
        for (int j = 0; j < tagNumber; j++)
        {
            if (Tags.Count == 0)
                break;
            int randomIndex = UnityEngine.Random.Range(0, Tags.Count);
            TagMask.Add(Tags[randomIndex]);
            Tags.RemoveAt(randomIndex);
        }
    }

    public void AddEntityResource(ResourceType type, int quantity)
    {
        if (_resourcesGained.ContainsKey(type) == false)
            return;

        _resourcesGained[type].Quantity += quantity;
        Debug.Log($"Added Entity {quantity} {type}. Total: {_resourcesGained[type].Quantity}");
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
            PlayerStats.Instance.AddResource(_resourcesGained[ResourceType.Gold]);
        }
        else
        {
            PlayerStats.Instance.AddResource(_resourcesGained[ResourceType.Fame]);
        }
        gameObject.SetActive(false);
        OnDeath?.Invoke(this);
    }
}
