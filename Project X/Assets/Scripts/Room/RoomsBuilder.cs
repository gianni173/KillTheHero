using Sirenix.OdinInspector;
using UnityEngine;

public class RoomsBuilder : MonoBehaviour
{
    [Header("Prefabs to spawn")] 
    [SerializeField, AssetsOnly] 
    private GameObject _roomsPrefab;

    [SerializeField] 
    private Transform _roomsContainer;

    private GridManager _gridManager;
    private GridManager GridManager
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
                
                var content = grid.GetGridContent(index);
                if (content is not RoomData roomData)
                {
                    continue;
                }
                
                var worldPos = grid.GridCoordToWorldCoord(coord);
                var roomObject = Instantiate(_roomsPrefab, worldPos, Quaternion.identity, _roomsContainer);
                var room = roomObject.GetComponent<Room>();
                if (room == null)
                {
                    Debug.LogError($"The prefab connected to {nameof(RoomsBuilder)} has no {nameof(Room)} component");
                    continue;
                }
                room.Init(roomData);
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