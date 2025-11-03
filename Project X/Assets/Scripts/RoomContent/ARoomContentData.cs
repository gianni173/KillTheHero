using System;
using UnityEngine;

[Serializable]
public abstract class ARoomContentData : IEntityInteractable
{
    public string Name;
    public Sprite Sprite;
    public EntityTag[] InteractableTagMask;
    public int MaxUsages = 999;
    public int Usages = 999;
    public bool IsUsable => Usages > 0;
    
    public virtual void Interact(Entity entity)
    {
        Usages--;
    }
}
