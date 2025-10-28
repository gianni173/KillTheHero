using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List <EntityTag> tagMask = new List<EntityTag>();
    //private Dictionary<ResourceType, Resource> ResourcesGained = new Dictionary<ResourceType, Resource>();

    /*public void AddResources(Resource resource)
    {
        ResourcesGained[resource.ResourceType] += resource.quantity ;
        //other things
    }*/

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
