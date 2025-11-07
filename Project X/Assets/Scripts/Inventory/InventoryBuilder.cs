using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class InventoryBuilder : SerializedMonoBehaviour
{
    public static InventoryBuilder Instance;

    [Header("Prefabs to spawn")] [SerializeField, AssetsOnly]
    private GameObject _RoomContentPrefab;

    private List<RoomContent> _roomContents = new();
    public List<RoomContent> RoomContents => _roomContents;
    [SerializeField] private Transform _container;
    public int MaxColumn = 5;
    [OdinSerialize, ReadOnly] private Inventory _inventory;
    private Vector3 _offset = Vector3.zero;

    private void Start()
    {
        Debug.Log("GridViewer Awake");
        _inventory = PlayerStats.Instance.PlayerInventory;
        if (_inventory != null)
        {
            Debug.Log("Player Inventory Found");
            _inventory.OnInventoryChanged += _ => Refresh();
            BuildGrid(_inventory.GetInventoryContents());
            Refresh();
            return;
        }

        Debug.Log("Player Inventory null");
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        for (int i = 0; i < list.Count; i++)
        {
            var purchasable = list[i];
            var obj = Instantiate(
                _RoomContentPrefab,
                transform.localPosition + _offset,
                Quaternion.identity,
                _container
            );
            var roomContent = obj.GetComponent<RoomContent>();
            if (_offset.x == MaxColumn)
            {
                _offset.y--;
                _offset.x = 0;

                continue;
            }

            _offset.x++;
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