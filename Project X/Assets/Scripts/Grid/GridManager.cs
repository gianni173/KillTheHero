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
}
