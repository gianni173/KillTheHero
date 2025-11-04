using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryViewer : MonoBehaviour
{
    [SerializeField] private GameObject _RoomContentPrefab;
    private Inventory _inventory;
    
    public int  MaxColumn = 5;

    private Vector3 _offset = Vector3.zero;

    private void Start()
    {
        Debug.Log("GridViewer Awake");
        _inventory = PlayerStats.Instance.GetInventory();
         _inventory.OnInventoryChanged.AddListener(Refresh);
        // Debug.Log(PlayerStats.Instance);
        if(_inventory != null) 
            BuildGrid(_inventory.GetInventoryContents());
    }

    public void Refresh()
    {
        DestroyGrid();
        BuildGrid(_inventory.GetInventoryContents());
    }
    public void BuildGrid(List<ARoomContentData> list)
    {
        foreach (var purchasable in list)
        {
            var obj = Instantiate(_RoomContentPrefab, transform.localPosition + _offset, Quaternion.identity , transform);
            var roomContent = obj.GetComponent<RoomContent>();
            roomContent.Init(purchasable);
            if (_offset.x == MaxColumn)
            {
                _offset.y--;
                _offset.x = 0;
                
                continue;
            }
            _offset.x++;
        }
        
    }

    public void DestroyGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _offset = Vector3.zero;
    }
}
