using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableRoomContent_", menuName = "Purchasable/RoomContent")]
public class PurchasableRoomContent : Purchasable
{
    public string Name;
    public ARoomContentData RoomContentType;
    
    public override void Purchase()
    {
        base.Purchase();
        
        PlayerStats.Instance.GetInventory().AddItemToInventory(Instantiate(RoomContentType));
    }
}
