using UnityEngine;
using Sirenix.OdinInspector;
public class PathFinderSystem : MonoBehaviour
{

    [SerializeField, ReadOnly] private int _currentIndex = 0;
    public int CurrentIndex
    {
        get
        {
            return _currentIndex;
        }
        set
        {
            _currentIndex = value;
        }
    }

    [SerializeField]
    private int _targetIndex = 20;
    public int TargetIndex
    {
        get
        {
            return _targetIndex;
        }
        set
        {
            _targetIndex = value;
        }
    }
    [SerializeField] private Path _currentPath;
    public Path CurrentPath => _currentPath;

    [SerializeField] private Grid _grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _grid = GridManager.Instance?.Grid;
        if (_grid == null)
        {
            _grid = Grid.DefaultGrid();
        }

        if (_grid == null)
        {
            Debug.LogError("[PathFinderSystem] Grid is not assigned and GridManager instance is null.");
            return;
        }

        UpdatePositionToCurrentStep();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FindPath()
    {
        if (_grid == null)
        {
            Debug.LogError("[PathFinderSystem] Grid is not assigned.");
            return;
        }

        _currentPath = PathFinder.AStarPathFinding(_grid, _currentIndex, _targetIndex);
        if (_currentPath != null)
        {
            Debug.Log("[PathFinderSystem] Path successfully found.");
        }
        else
        {
            Debug.LogWarning("[PathFinderSystem] No path could be found.");
        }
    }

    [Button]
    public void UpdatePath()
    {
        _currentPath = null;
        FindPath();
        UpdatePositionToCurrentStep();
    }

    [Button]
    public int NextStep()
    {
        if (_currentPath == null || !_currentPath.HasNextStep())
        {
            Debug.LogWarning("[PathFinderSystem] No current path to follow.");
            return -1;
        }

        _currentIndex = _currentPath.GetNextStep();
        UpdatePositionToCurrentStep();

        return _currentIndex;
    }

    public void UpdatePositionToCurrentStep()
    {
        if (_currentPath == null)
        {
            Debug.LogWarning("[PathFinderSystem] No current path to follow.");
            return;
        }

        Vector3 worldPos = _grid.GridCoordToWorldCoord(_grid.IndexToGridCoord(_currentIndex));
        Vector3 offset = new Vector3(_grid.WorldCellSize.x / 2, _grid.WorldCellSize.y / 2, 0);
        transform.position = worldPos + offset;
    }

    public void SetPath(Path path)
    {
        _currentPath = path;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
