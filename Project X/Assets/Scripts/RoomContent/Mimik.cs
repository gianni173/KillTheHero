using UnityEngine;

[CreateAssetMenu(fileName = "RoomContent_Mimik", menuName = "RoomContent/Mimik")]
public class Mimik : ARoomContentData
{
    public override void Interact(Entity entity)
    {
        entity.Die();
    }
}
