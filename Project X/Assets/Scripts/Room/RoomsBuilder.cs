using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomsBuilder : MonoBehaviour
{
    [Header("Prefabs to spawn")] 
    [SerializeField, AssetsOnly] 
    private GameObject _roomsPrefab;

    [SerializeField, AssetsOnly]
    private GameObject _roomSlot;

    [SerializeField] 
    private Transform _roomsContainer;

    private List<Room> _rooms = new();
    private List<RoomContent> _roomContents = new();
    public List<Room> Rooms => _rooms;
    public List<RoomContent> RoomContents => _roomContents;
    private GridManager _gridManager;
    public GridManager GridManager
    {
        get
        {
            _gridManager = GridManager.Instance;
            return _gridManager;
        }
    }
    
    private void Start()
    {
        if (GridManager == null)
        {
            enabled = false;
            return;
        }

        GridManager.Grid.OnChanged += _ => Refresh();
        
        Refresh();
    }
    
    public void Refresh()
    {
        DestroyRooms();
        BuildRooms();
    }
    
    private void BuildRooms()
    {
        var grid = GridManager.Grid;
        var size = grid.CurrentSize;

        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                var coord = new Vector2Int(x, y);
                var index = grid.GridCoordToIndex(coord);
                
                // check slot in grid coordinates x,y
                Vector3 pos = grid.GridCoordToWorldCoord(new Vector2Int(x, y));
                // immediately instantiate a free slot
                Instantiate(_roomSlot, pos, Quaternion.identity, _roomsContainer);
                
                // check if there is a room in the grid coordinates x,y, if not, go to the next cycle
                var content = grid.GetGridContent(index);
                if (content is not RoomData roomData)
                {
                    continue;
                }
                // otherwise, turn the grid coordinates into world position, and then instantiate a room under _roomsContainer's transform parent
                var worldPos = grid.GridCoordToWorldCoord(coord);
                var roomObject = Instantiate(_roomsPrefab, worldPos, Quaternion.identity, _roomsContainer);
                var room = roomObject.GetComponent<Room>();
                // quick check if the prefab room has no room component
                if (room == null)
                {
                    Debug.LogError($"The prefab connected to {nameof(RoomsBuilder)} has no {nameof(Room)} component");
                    continue;
                }
                // register the room to the drag system, and initialize the room data
                RoomDraggableSystem.Instance.RegisterDraggable(room);
                _rooms.Add(room);
                room.Init(roomData);
                //TODO: add to _roomContents
                GridManager.Grid.GetAvailableGridIndices();
            }
        }
    }

    private void DestroyRooms()
    {
        foreach (var child in _roomsContainer.GetComponentsInChildren<Transform>())
        {
            if (child.parent == _roomsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}