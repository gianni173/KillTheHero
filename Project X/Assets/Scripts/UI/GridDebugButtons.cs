using UnityEngine;
using UnityEngine.UI;

public class GridDebugButtons : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] 
    private Button _addWidth;
    
    [SerializeField] 
    private Button _addHeight;

    private GridManager GridManager => GridManager.Instance;
    
    
    private void Awake()
    {
        _addWidth?.onClick.AddListener(AddWidth);
        _addHeight?.onClick.AddListener(AddHeight);
    }

    private void AddWidth()
    {
        GridManager.SetCurrentGridSize(GridManager.Grid.CurrentSize + Vector2Int.right);
    }

    private void AddHeight()
    {
        GridManager.SetCurrentGridSize(GridManager.Grid.CurrentSize + Vector2Int.up);
    }
}
