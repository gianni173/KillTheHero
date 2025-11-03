public class Monster : ARoomContentData
{
    private int _fameReward;
    private EntityTag[] _killTagMask;

    public override void Interact(Entity entity)
    {
        if(IsUsable == false)
            return;
        if(entity.CheckEntityTag(_killTagMask))
            Kill(entity);
        if (entity.CheckEntityTag(InteractableTagMask))
            entity.Die();
    }

    public void Kill(Entity entity)
    {
        //GetComponent<Renderer>().enabled = false; 
        //Jachy Hu 30/10: doesn't work because AroomContentData it's just a plain class
        //Integrate it into RoomContent, maybe with _isUsable OnChange event?
        entity.AddEntityResource(ResourceType.Fame, _fameReward);
        IsUsable = false;
    }
}
