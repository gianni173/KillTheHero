using System;
using UnityEngine;

[Serializable]
public abstract class ARoomContentData : IEntityInteractable
{
    public Sprite Sprite;
    protected string Name;
    protected int Usage;
    protected EntityTag[] InteractableTagMask;
    protected bool IsUsable = true;
    
    public virtual void Interact(Entity entity)
    {
        //TODO
        throw new NotImplementedException();
    }

    public void Reset()
    {
        IsUsable = true;
    }
}
