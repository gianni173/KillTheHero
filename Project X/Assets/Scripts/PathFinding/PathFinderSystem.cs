using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting.Dependencies.NCalc;
public class PathFinderSystem : MonoBehaviour
{

    [SerializeField] private int _currentIndex = 0;
    public int CurrentIndex => _currentIndex;

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

        ResetPath();
        UpdatePositionToCurrentStep();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FindPath(int startIndex, int targetIndex)
    {
        if (_grid == null)
        {
            Debug.LogError("[PathFinderSystem] Grid is not assigned.");
            return;
        }

        _currentPath = PathFinding.AStarPathFinding(_grid, startIndex, targetIndex);
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
    public void ResetPath()
    {
        _currentPath = null;
        _currentIndex = 0;
        FindPath(_currentIndex, 20);
        UpdatePositionToCurrentStep();
    }

    [Button]
    public void NextStep()
    {
        if (_currentPath == null || !_currentPath.HasNextStep())
        {
            Debug.LogWarning("[PathFinderSystem] No current path to follow.");
            return;
        }

        _currentIndex = _currentPath.GetNextStep();
        UpdatePositionToCurrentStep();
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
