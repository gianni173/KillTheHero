public class Trap : ARoomContentData
{
    private EntityTag[] _disarmTagMask;

    public override void Interact(Entity entity)
    {
        if (IsUsable == false)
        {
            return;
        }

        if (entity.CheckEntityTag(_disarmTagMask))
        {
            Disarm();
        }

        if (entity.CheckEntityTag(InteractableTagMask))
        {
            entity.Die();
        }
    }

    public void Disarm()
    {
        //add logic to disarm the trap
        IsUsable = false;
    }
}