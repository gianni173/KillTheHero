using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class PathFinderSystem : MonoBehaviour
{

    [SerializeField]
    private int currentIndex = 0;
    public int CurrentIndex => currentIndex;

    [SerializeField]
    private Path _currentPath;
    public Path CurrentPath => _currentPath;

    [SerializeField]
    private Grid _grid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_grid == null)
        {
            _grid = GridManager.Instance?.Grid;
        }

        if (_grid == null)
        {
            Debug.LogError("[PathFinderSystem] Grid is not assigned and GridManager instance is null.");
            return;
        }
        
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
        currentIndex = 0;
        FindPath(currentIndex, 20);
    }

    [Button]
    public void NextStep()
    {
        if (_currentPath == null || !_currentPath.HasNextStep())
        {
            Debug.LogWarning("[PathFinderSystem] No current path to follow.");
            return;
        }
        
        currentIndex = _currentPath.GetNextStep();
        Vector3 worldPos = _grid.GridCoordToWorldCoord(_grid.IndexToGridCoord(currentIndex));
       
    }
}
