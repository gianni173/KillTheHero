using UnityEngine;

[CreateAssetMenu(fileName = "RoomContent_Monster", menuName = "RoomContent/Monster")]
public class Monster : ARoomContentData
{
    [SerializeField]
    private int _fameReward;
    [SerializeField]
    private EntityTag[] _killTagMask;

    public override void Interact(Entity entity)
    {
        if(IsUsable == false)
            return;
        if(entity.CheckEntityTag(_killTagMask))
            Kill(entity);
        if (entity.CheckEntityTag(InteractableTagMask))
        {
            PlayerStats.Instance.AddResource(entity.ResourcesGained[ResourceType.Fame]);
            entity.Die();
        }
    }

    public void Kill(Entity entity)
    {
        //GetComponent<Renderer>().enabled = false; 
        //Jachy Hu 30/10: doesn't work because AroomContentData it's just a plain class
        //Integrate it into RoomContent, maybe with _isUsable OnChange event?
        entity.AddEntityResource(ResourceType.Fame, _fameReward);
    }
}
