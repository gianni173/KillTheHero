using UnityEngine;

public class Mimik : ARoomContentData
{
    public override void Interact(Entity entity)
    {
        entity.Die();
    }
}
