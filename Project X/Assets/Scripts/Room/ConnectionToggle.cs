using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectionToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;
    public GameObject Room => _parent;

    private int _roomIndex;
    private int _connectionIndex;

    [SerializeField]
    private GameObject _connectedVisual;

    [SerializeField]
    private LayerMask _connectionLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, _connectionLayerMask);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                var Grid = GridManager.Instance.Grid;
                Grid.ToggleConnection(_roomIndex, _connectionIndex);
            }
        }
    }
    public void Init()
    {
        if (Room == null)
        {
            Debug.LogWarning("Room or Room Data or Connections is null");
            return;
        }

        var Grid = GridManager.Instance.Grid;
        var roomCoord = Grid.WorldCoordToGridCoord(Room.transform.position);
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
        if (Room == null)
        {
            Debug.LogWarning("Room or Room Data or Connections is null");
            return false;
        }
        var Grid = GridManager.Instance.Grid;

        return Grid.CheckConnectionIsValid(_roomIndex, _connectionIndex);
    }
    
    public void UpdateVisual()
    {
        bool isConnected = CheckConnection();
        if(_connectedVisual != null)
            _connectedVisual.SetActive(!isConnected);
    }
}
