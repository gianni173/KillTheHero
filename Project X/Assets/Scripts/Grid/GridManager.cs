using UnityEngine;

public  class GridManager : MonoBehaviour
{
    [SerializeField] private Grid _grid = null;
    public static GridManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        if (_grid.MaxSize == Vector2Int.zero)
        {
            _grid = Grid.DefaultGrid();
        }
    }
    public void SetCurrentGridSize(Vector2Int newSize)
    {
        if (_grid != null)
        {
            _grid.SetCurrentSize(newSize);
        }
    }

    public Grid GetGrid()
    {
        return _grid;
    }

    void OnDrawGizmos()
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
            int[] gridIndices = _grid.GetAvailableGridIndices();
            for(int i = 0; i <gridIndices.Length; i++)
            {
                Vector2 cellCoord = Grid.IndexToGridCoord(gridIndices[i]);
                Vector3 cellPos = _grid.GridCoordToWorldCoord(cellCoord);
                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(cellPos + cellOffset, cellDimention);
            }
        }
    }
}
