using UnityEngine;

[CreateAssetMenu(fileName = "RoomContent_Mimik", menuName = "RoomContent/Mimik")]
public class Mimik : ARoomContentData
{
    public override void Interact(Entity entity)
    {
        PlayerStats.Instance.AddResource(entity.ResourcesGained[ResourceType.Gold]);
        entity.Die();
    }
}
