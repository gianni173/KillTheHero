using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Room _room;
    public Room Room => _room;

    private int _roomIndex;
    private int _connectionIndex;

    [SerializeField]
    private GameObject _connectedVisual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Right) return;
        var Grid = GridManager.Instance.Grid;
        Grid.ToggleConnection(_roomIndex, _connectionIndex);
    }

    public void Init()
    {
        if (_room == null)
        {
            Debug.LogWarning("Room or Room Data or Connections is null");
            return;
        }

        var Grid = GridManager.Instance.Grid;
        var roomCoord = Grid.WorldCoordToGridCoord(_room.transform.position);
        _roomIndex = Grid.GridCoordToIndex(roomCoord);

        var connectedGrid = Vector2Int.zero;
        if (transform.localPosition.x > 0)
        {
            connectedGrid.x = 1;
        }
        else if (transform.localPosition.x < 0)
        {
            connectedGrid.x = -1;
        }

        if (transform.localPosition.y > 0)
        {
            connectedGrid.y = 1;
        }
        else if (transform.localPosition.y < 0)
        {
            connectedGrid.y = -1;
        }

        var targetCoord = roomCoord + connectedGrid;
        _connectionIndex = Grid.GridCoordToIndex(targetCoord);
        UpdateVisual();
    }

    public bool CheckConnection()
    {
        if (_room == null)
        {
            Debug.LogWarning("Room or Room Data or Connections is null");
            return false;
        }
        var Grid = GridManager.Instance.Grid;
        bool isConnected = Grid.Connections.ContainsKey(_roomIndex) &&
                           Grid.Connections[_roomIndex].Contains(_connectionIndex);

        // Also check if the neighbor room has a connection back to this room and that the neighbor room exists
        bool isNeighborConnected = Grid.Connections.ContainsKey(_connectionIndex) &&
                           Grid.Connections[_connectionIndex].Contains(_roomIndex)
                           && Grid.GetGridContent(_connectionIndex) != null;

        return isConnected && isNeighborConnected;
    }
    
    public void UpdateVisual()
    {
        bool isConnected = CheckConnection();
        if(_connectedVisual != null)
            _connectedVisual.SetActive(!isConnected);
    }
}
