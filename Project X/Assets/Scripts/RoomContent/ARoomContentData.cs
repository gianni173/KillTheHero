
public abstract class ARoomContentData : IEntityInteractable
{
    protected string _name;
    protected int _usage;
    protected EntityTag[] _interactableTagMask;
    
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
