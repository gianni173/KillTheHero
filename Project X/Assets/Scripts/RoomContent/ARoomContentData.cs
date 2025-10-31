using System;

[Serializable]
public abstract class ARoomContentData : IEntityInteractable
{
    protected string _name;
    protected int _usage;
    protected EntityTag[] _interactableTagMask;
    protected bool _isUsable = true;
    
    public virtual void Interact(Entity entity)
    {
        //TODO
        throw new NotImplementedException();
    }

    public void Reset()
    {
        _isUsable = true;
    }
}
