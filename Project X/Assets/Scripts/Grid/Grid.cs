using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grid
{
    [Header("Grid Dimensions")]
    [SerializeField] private Vector2Int _maxSize;     // Dimensione massima griglia
    [SerializeField] private Vector2Int _currentSize;   // Dimensione attualmente accessibile al player
    
    [Header("World Settings")]
    [SerializeField] private Vector3 _origin = Vector3.zero;
    [SerializeField] private Vector2 _worldCellSize = Vector2.one;
    
    [Header("Player Resources")]
    [SerializeField] private int _availableTiles;
    
    
    private Dictionary<int, AGridContent> _content = new Dictionary<int, AGridContent>();
    private Dictionary<int, int[]> _connections = new Dictionary<int, int[]>();
    
    
    public static Vector2 IndexToGridCoord(int index)
    {
        var grid = GridManager.Instance.GetGrid();
        int X = index % grid._maxSize.x;
        int Y = Mathf.FloorToInt(index / grid._maxSize.x);
        return new Vector2(X, Y);
    }

    public static int GridCoordToIndex(Vector2Int coord)
    {
        var grid = GridManager.Instance.GetGrid();
        return coord.y * grid._maxSize.x + coord.x;
    }

    public Vector3 GridCoordToWorldCoord(Vector2 coord)
    {
        return new Vector3(
            _origin.x + coord.x * _worldCellSize.x,
            _origin.y + coord.y * _worldCellSize.y,
            _origin.z
        );
    }
    public AGridContent GetGridContent(int index)
    {
        return _content[index];
    }

    public void RemoveGridContent(int index)
    {
        //Salvo la reference per un futuro Destroy() o SetActive(false)
        //AGridContent gridContent = _content[index]; 
        _content.Remove(index);
    }

    public void AddContent(int index, AGridContent content)
    {
        _content.Add(index , content);
    }
}

