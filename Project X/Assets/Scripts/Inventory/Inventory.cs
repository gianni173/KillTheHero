using System.Collections.Generic;

public class Inventory
{
    private List<PurchasableRoomContent>  _purchasedRoomContents;

    public List<PurchasableRoomContent> GetInventoryContents()
    {
        return _purchasedRoomContents;
    }

    public void AddItemToInventory(PurchasableRoomContent item)
    {
        _purchasedRoomContents.Add(item);
    }

    public void RemoveItemFromInventory(PurchasableRoomContent item)
    {
        _purchasedRoomContents.Remove(item);
    }
    
    //TODO: add logic to instantiate inventory UI whenever the contents change.
}
