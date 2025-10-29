
public abstract class ARoomContentData : IEntityInteractable
{
    protected string _name;
    protected int _usage;
    protected EntityTag[] _interactableTagMask;
    protected bool _isUsable;
    
    public virtual void Interact(Entity entity)
    {
        //to do
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        //to do
        throw new System.NotImplementedException();
    }
}
