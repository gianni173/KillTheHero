using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class InventoryBuilder : SerializedMonoBehaviour
{
    [Header("Prefabs to spawn")] 
    [SerializeField, AssetsOnly] 
    private GameObject _RoomContentPrefab;
    private List<RoomContent> _roomContents = new();
    public List<RoomContent> RoomContents => _roomContents;
    [SerializeField]
    private Transform _container;
    [OdinSerialize, ReadOnly]
    private Inventory _inventory;
    private Vector3 _offset = Vector3.zero;
    
    private void Start()
    {
        Debug.Log("GridViewer Awake");
        _inventory = PlayerStats.Instance.PlayerInventory;
        if (_inventory != null)
        {
            BuildGrid(_inventory.GetInventoryContents());
            return;
        }
        Debug.Log("Player Inventory Empty");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        DestroyGrid();
        BuildGrid(_inventory.GetInventoryContents());
    }

    public void BuildGrid(List<ARoomContentData> list)
    {
        //cycle each 
        foreach (var purchasable in list)
        {
            var obj = Instantiate(
                _RoomContentPrefab, 
                transform.localPosition,
                Quaternion.identity, 
                _container
                );
            var roomContent = obj.GetComponent<RoomContent>();
            // register the roomContent to the drag system, and initialize the roomContent data
            RoomContentDraggableSystem.Instance.RegisterDraggable(roomContent);
            RoomContents.Add(roomContent);
            roomContent.Init(purchasable);
        }
    }

    public void DestroyGrid()
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }
        _offset = Vector3.zero;
    }
}