using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class GridManager : SerializedMonoBehaviour
{
    [OdinSerialize] private Grid _grid = null;
    public Grid Grid => _grid;

    public static GridManager Instance;

    [SerializeField]
    private bool _activateDebugGizmos = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
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
        if (!_activateDebugGizmos)
        {
            return;
        }

        if (_grid == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        // Draw grid
        var cellOffset = new Vector3(_grid.WorldCellSize.x / 2, _grid.WorldCellSize.y / 2, 0);
        var cellDimention = new Vector3(_grid.WorldCellSize.x, _grid.WorldCellSize.y, 0);
        for (var x = 0; x < _grid.MaxSize.x; x++)
        {
            for (var y = 0; y < _grid.MaxSize.y; y++)
            {
                var cellPos = _grid.GridCoordToWorldCoord(new Vector2Int(x, y));
                Gizmos.DrawWireCube(cellPos + cellOffset, cellDimention);
            }
        }

        var _gridIndices = _grid.GetAvailableGridIndices();
        for (var i = 0; i < _gridIndices.Length; i++)
        {
            var cellCoord = Grid.IndexToGridCoord(_gridIndices[i]);
            var cellPos = _grid.GridCoordToWorldCoord(cellCoord);
            Gizmos.color = Color.black;
            // Grid cell
            Gizmos.DrawWireCube(cellPos + cellOffset, cellDimention);
            var content = _grid.GetGridContent(_gridIndices[i]);
            if (content == null)
            {
                continue;
            }

            //Grid Center
            Gizmos.DrawSphere(cellPos + cellOffset, .2f);
            if (!_grid.Connections.ContainsKey(_gridIndices[i]))
            {
                continue;
            }
            // connections
            var connections = _grid.Connections[_gridIndices[i]];
            Gizmos.color = Color.blue;
            for (var j = 0; j < connections.Length; j++)
            {
                var connCellCoord = Grid.IndexToGridCoord(connections[j]);
                var connCellPos = _grid.GridCoordToWorldCoord(connCellCoord);
                Gizmos.DrawLine(cellPos + cellOffset, connCellPos + cellOffset);
            }
        }
    }
}