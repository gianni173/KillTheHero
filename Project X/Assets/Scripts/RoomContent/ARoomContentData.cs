using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class ARoomContentData : ScriptableObject, IEntityInteractable
{
    public string Name;
    public Sprite Sprite;
    public EntityTag[] InteractableTagMask;
    public int MaxUsages = 999;
    [HideInEditorMode]
    public int Usages = 999;
    public bool IsUsable => Usages > 0;
    
    public virtual void Interact(Entity entity)
    {
        Usages--;
    }
}
