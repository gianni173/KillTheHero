using UnityEngine;

public class PurchasableRoomContent : Purchasable
{
    public string Name;
    public ARoomContentData RoomContentType;
    
    public override void Purchase()
    {
        base.Purchase();
        
        PlayerStats.Instance.GetInventory().AddItemToInventory(RoomContentType);
    }
}
