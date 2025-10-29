
public abstract class ARoomContentData : IEntityInteractable
{
    private string _name;
    private int _usage;
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
