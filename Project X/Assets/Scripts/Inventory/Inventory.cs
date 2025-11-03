using System.Collections.Generic;
using Sirenix.OdinInspector;

public class Inventory
{
    [ShowInInspector, DisableInEditorMode]
    private List<ARoomContentData>  _purchasedRoomContents = new();
    
    public List<ARoomContentData> GetInventoryContents()
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        return _purchasedRoomContents;
    }

    public void AddItemToInventory(ARoomContentData item)
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        _purchasedRoomContents.Add(item);
    }

    public void RemoveItemFromInventory(ARoomContentData item)
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        _purchasedRoomContents.Remove(item);
    }
    
    //TODO: add logic to instantiate inventory UI whenever the contents change.
}
