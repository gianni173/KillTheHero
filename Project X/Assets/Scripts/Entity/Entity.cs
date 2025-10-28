using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityTag
    {
        Paladin,
        Assassin,
        Strong,
        Weak
    };
    
    public List <EntityTag> tagMask = new List<EntityTag>();
    //private Dictionary<ResourceType, Resource> ResourcesGained;

    /*public void AddResources(Resource resource)
    {
        resource.quantity++;
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
