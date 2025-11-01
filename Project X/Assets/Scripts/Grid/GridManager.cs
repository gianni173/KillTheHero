using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class GridManager : SerializedMonoBehaviour
{
    [OdinSerialize] private Grid _grid = null;
    public Grid Grid => _grid;

    public static GridManager Instance;

    [SerializeField, ReadOnly] 
    private int[] _gridIndices;

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

    private void Start()
    {
        if (_grid.MaxSize == Vector2Int.zero)
        {
            _grid = Grid.DefaultGrid();
        }
    }

    public void SetCurrentGridSize(Vector2Int newSize)
    {
        if (_grid == null)
        {
            return;
        }
        _grid.SetCurrentSize(newSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_grid != null)
        {
            Vector3 cellOffset = new Vector3(_grid.WorldCellSize.x / 2, _grid.WorldCellSize.y / 2, 0);
            Vector3 cellDimention = new Vector3(_grid.WorldCellSize.x, _grid.WorldCellSize.y, 0);
            for (int x = 0; x < _grid.MaxSize.x; x++)
            {
                for (int y = 0; y < _grid.MaxSize.y; y++)
                {
                    Vector3 cellPos = _grid.GridCoordToWorldCoord(new Vector2(x, y));
                    Gizmos.DrawWireCube(cellPos + cellOffset, cellDimention);
                }
            }
            _gridIndices = _grid.GetAvailableGridIndices();
            for(int i = 0; i <_gridIndices.Length; i++)
            {
                Vector2 cellCoord = Grid.IndexToGridCoord(_gridIndices[i]);
                Vector3 cellPos = _grid.GridCoordToWorldCoord(cellCoord);
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(cellPos + cellOffset, cellDimention);
                AGridContent content = _grid.GetGridContent(_gridIndices[i]);
                if(content != null)
                {
                    // Gizmos.color = Color.black;
                    Gizmos.DrawSphere(cellPos + cellOffset, .2f);
                    if(_grid.Connections.ContainsKey(_gridIndices[i]))
                    {
                        int[] connections = _grid.Connections[_gridIndices[i]];
                        Gizmos.color = Color.blue;
                        for(int j = 0; j < connections.Length; j++)
                        {
                            Vector2 connCellCoord = Grid.IndexToGridCoord(connections[j]);
                            Vector3 connCellPos = _grid.GridCoordToWorldCoord(connCellCoord);
                            Gizmos.DrawLine(cellPos + cellOffset, connCellPos + cellOffset);
                        }
                    }
                }
            }
        }
    }
}
