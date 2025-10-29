using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List <EntityTag> tagMask = new List<EntityTag>();
    private Dictionary<ResourceType, Resource> ResourcesGained = new Dictionary<ResourceType, Resource>();

    public void AddResources(Resource resource)
    {
        // Adding resources logic
        if (ResourcesGained.ContainsKey(resource.Type))
            ResourcesGained[resource.Type].Quantity += resource.Quantity;
        else
            ResourcesGained[resource.Type] = resource;
        //other things
    }

    // Check if the entity has a specific tag
    public bool CheckEntityTag(EntityTag enemyEntityTag)
    {
        return tagMask.Contains(enemyEntityTag);
    }

    public bool CheckEntityTag(List<EntityTag> enemyEntityTags)
    {
        foreach (EntityTag tag in enemyEntityTags)
        {
            if (tagMask.Contains(tag))
                return true;
        }
        return false;
    }

}
