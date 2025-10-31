using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List <EntityTag> TagMask = new();
    private Resource[] _resourcesGained;


    public void AddResources(Resource resource)
    {
        // Adding resources logic
        if (_resourcesGained.ContainsKey(resource.Type))
            _resourcesGained[resource.Type].Quantity += resource.Quantity;
        else
            _resourcesGained[resource.Type] = resource;
        //other things
    }

    // Check if the entity has a specific tag
    public bool CheckEntityTag(EntityTag enemyEntityTag)
    {
        return TagMask.Contains(enemyEntityTag);
    }

    public bool CheckEntityTag(List<EntityTag> enemyEntityTags)
    {
        foreach (EntityTag tag in enemyEntityTags)
        {
            if (TagMask.Contains(tag))
                return true;
        }
        return false;
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
