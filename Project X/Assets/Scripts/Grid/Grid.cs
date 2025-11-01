using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grid
{
    [Header("Grid Dimensions")]
    [SerializeField] private Vector2Int _maxSize;     // max grid dimension
    public Vector2Int MaxSize => _maxSize;
    [SerializeField] private Vector2Int _currentSize;   // current grid dimension visible to the player
    
    [Header("World Settings")]
    [SerializeField] private Vector3 _origin = Vector3.zero;
    [SerializeField] private Vector2 _worldCellSize = Vector2.one;
    public Vector2 WorldCellSize => _worldCellSize;


    [SerializeField] private Dictionary<int, AGridContent> _content = new();
    [SerializeField] private Dictionary<int, int[]> _connections = new();
    public Dictionary<int, int[]> Connections => _connections;
    
    
    public Vector2 IndexToGridCoord(int index)
    {
        int X = index % _maxSize.x;
        int Y = Mathf.FloorToInt(index / _maxSize.x);
        return new Vector2(X, Y);
    }

    public int GridCoordToIndex(Vector2Int coord)
    {
        return coord.y * _maxSize.x + coord.x;
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
        if (!_content.ContainsKey(index))
            return null;
        return _content[index];
    }

    public void RemoveGridContent(int index)
    {
        //TODO: Salvo la reference per un futuro Destroy() o SetActive(false)
        //AGridContent gridContent = _content[index]; 
        _content.Remove(index);
    }

    public void AddContent(int index, AGridContent content)
    {
        _content.Add(index , content); 
    }

    public void SetCurrentSize(Vector2Int newSize)
    {
        // check size 
        if (newSize.x <= 0 || newSize.y <= 0)
        {
            Debug.LogError($"new grid dimension not valid: {newSize}.");
            return;
        }

        if (newSize.x > _maxSize.x || newSize.y > _maxSize.y)
        {
            Debug.LogError($"new grid dimension not valid: ({newSize}) bigger than ({_maxSize})!");
            return;
        }

        // Se arriviamo qui, i valori sono validi
        _currentSize = newSize;
        Debug.Log($"[Grid] Visible grid updated at: {_currentSize}");
    }

    public int[] GetAvailableGridIndices()
    {
        var availableIndices = new List<int>();
        for (int x = 0; x < _currentSize.x; x++)
        {
            for (int y = 0; y < _currentSize.y; y++)
            {
                int index = GridCoordToIndex(new Vector2Int(x, y));
                availableIndices.Add(index);
            }
        }

        return availableIndices.ToArray();
    }

    static public Grid DefaultGrid()
    {
        return new Grid
        {
            _maxSize = new Vector2Int(10, 10),
            _currentSize = new Vector2Int(3, 3),
            _origin = Vector3.zero,
            _worldCellSize = Vector2.one
        };
    }
    
    public int[] GetNeighborsIndices(int index)
    {
        Vector2 coord = IndexToGridCoord(index);
        List<int> neighbors = new List<int>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue; // Skip the current cell

                Vector2 neighborCoord = new Vector2(coord.x + x, coord.y + y);

                // Check if neighbor is within bounds
                if (neighborCoord.x >= 0 && neighborCoord.x < _maxSize.x &&
                    neighborCoord.y >= 0 && neighborCoord.y < _maxSize.y)
                {
                    int neighborIndex = GridCoordToIndex(new Vector2Int((int)neighborCoord.x, (int)neighborCoord.y));
                    neighbors.Add(neighborIndex);
                }
            }
        }
        return neighbors.ToArray();
    }
}
