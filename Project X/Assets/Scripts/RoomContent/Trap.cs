using UnityEngine;

public class Trap : ARoomContentData
{
    private EntityTag[] _disarmTagMask;
    
    public override void Interact(Entity entity)
    {
        if(_isUsable == false)
            return;
        if(entity.CheckEntityTag(_disarmTagMask))
            Disarm();
        if (entity.CheckEntityTag(_interactableTagMask))
            entity.Die();
    }

    public void Disarm()
    {
        //add logic to disarm the trap
        _isUsable = false;
    }
}
