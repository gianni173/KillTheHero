using System.Collections.Generic;

public class Inventory
{
    private List<ARoomContentData>  _purchasedRoomContents;
    

    public List<ARoomContentData> GetInventoryContents()
    {
        return _purchasedRoomContents;
    }

    public void AddItemToInventory(ARoomContentData item)
    {
        _purchasedRoomContents.Add(item);
    }

    public void RemoveItemFromInventory(ARoomContentData item)
    {
        _purchasedRoomContents.Remove(item);
    }
    
    //TODO: add logic to instantiate inventory UI whenever the contents change.
}
