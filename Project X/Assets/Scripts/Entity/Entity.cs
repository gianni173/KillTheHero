using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List <EntityTag> tagMask = new List<EntityTag>();
    private Dictionary<ResourceType, Resource> ResourcesGained = new Dictionary<ResourceType, Resource>();

    public void AddResources(Resource resource)
    {
        foreach (var item in ResourcesGained)
        {
            if (item.Key == resource.Type)
            {
                item.Value.Quantity += resource.Quantity;
                return;
            }
        }
        //codice vecchio probabilmente sbagliato ResourcesGained[resource.Type] = resource.Quantity ;
        //other things
    }

    public bool CheckEntityTag(EntityTag enemyEntityTag)
    {
        if(tagMask.Contains(enemyEntityTag))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
