using UnityEngine;

public class Mimik : ARoomContentData
{
    public override void Interact(Entity entity)
    {
        //add logic to kill the hero and add gold to resources.
        entity.Die();
    }
}
