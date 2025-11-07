using System;
using System.Collections.Generic;
using Sirenix.Serialization;

public class Inventory
{
    public Action<Inventory>OnInventoryChanged;

    [OdinSerialize] private List<ARoomContentData> _purchasedRoomContents;
    
    public List<ARoomContentData> GetInventoryContents()
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        return _purchasedRoomContents;
    }

    public void AddItemToInventory(ARoomContentData item)
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        _purchasedRoomContents.Add(item);
        OnInventoryChanged?.Invoke(this);
    }

    public void RemoveItemFromInventory(ARoomContentData item)
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        _purchasedRoomContents.Remove(item);
        OnInventoryChanged?.Invoke(this);
    }
}
