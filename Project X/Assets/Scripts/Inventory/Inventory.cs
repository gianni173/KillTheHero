using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    [ShowInInspector, DisableInEditorMode]
    private List<ARoomContentData>  _purchasedRoomContents = new();
    public UnityEvent OnInventoryChanged = new();
    
    public List<ARoomContentData> GetInventoryContents()
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        return _purchasedRoomContents;
    }

    public void AddItemToInventory(ARoomContentData item)
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        _purchasedRoomContents.Add(item);
        Debug.Log(OnInventoryChanged);
        OnInventoryChanged.Invoke();
    }

    public void RemoveItemFromInventory(ARoomContentData item)
    {
        _purchasedRoomContents ??= new List<ARoomContentData>();
        _purchasedRoomContents.Remove(item);
        OnInventoryChanged.Invoke();
    }
    
    //TODO: add logic to instantiate inventory UI whenever the contents change.
}
