using Unity.VisualScripting;
using UnityEngine;

public class Monster : ARoomContentData
{
    private int _fameReward;
    private EntityTag[] _killTagMask;

    public override void Interact(Entity entity)
    {
        if(entity.CheckEntityTag(_killTagMask))
            Kill();
        if (entity.CheckEntityTag(_interactableTagMask))
            entity.Die();
    }

    public void Kill()
    {
        //add logic to kill the monster itself
        throw new System.NotImplementedException();
    }
}
