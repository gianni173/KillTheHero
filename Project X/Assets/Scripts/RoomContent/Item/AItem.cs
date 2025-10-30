using UnityEngine;

public class AItem : ARoomContentData
{
    public override void Interact(Entity entity)
    {
        if (_isUsable == false)
            return;
        if (entity.CheckEntityTag(_interactableTagMask))
        {
            //add collection logic to add the item to the entity's inventory
            Collect(entity);
        }
    }

    public virtual void Collect(Entity entity)
    {
        //add logic to use the item
        _isUsable = false;
    }
}
