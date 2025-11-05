using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class Grid
{
    public Action<Grid> OnChanged;

    [Header("Grid Dimensions")] [SerializeField]
    private Vector2Int _maxSize; // max grid dimension

    public Vector2Int MaxSize => _maxSize;
    [SerializeField] private Vector2Int _currentSize; // current grid dimension visible to the player
    public Vector2Int CurrentSize => _currentSize;

    [Header("World Settings")] [SerializeField]
    private Vector3 _origin = Vector3.zero;

    [SerializeField] private Vector2 _worldCellSize = Vector2.one;
    public Vector2 WorldCellSize => _worldCellSize;


    [OdinSerialize] private Dictionary<int, AGridContent> _content = new();
    public Dictionary<int, AGridContent> Content => _content;
    [OdinSerialize] private Dictionary<int, int[]> _connections = new();
    public Dictionary<int, int[]> Connections => _connections;
    
    public static Grid DefaultGrid()
    {
        return new Grid
        {
            _maxSize = new Vector2Int(10, 10),
            _currentSize = new Vector2Int(3, 3),
            _origin = Vector3.zero,
            _worldCellSize = Vector2.one
        };
    }

    #region Grid position helpers
    public Vector2Int IndexToGridCoord(int index)
    {
        var X = index % _maxSize.x;
        var Y = Mathf.FloorToInt(index / _maxSize.x);
        return new Vector2Int(X, Y);
    }

    public int GridCoordToIndex(Vector2Int coord)
    {
        return coord.y * _maxSize.x + coord.x;
    }

    public Vector3 GridCoordToWorldCoord(Vector2Int coord)
    {
        return new Vector3(
            _origin.x + coord.x * _worldCellSize.x,
            _origin.y + coord.y * _worldCellSize.y,
            _origin.z
        );
    }
    
    public Vector2Int WorldCoordToGridCoord(Vector3 worldCoord)
    {
        var coord = new Vector2Int(
            Mathf.FloorToInt((worldCoord.x - _origin.x) / _worldCellSize.x),
            Mathf.FloorToInt((worldCoord.y - _origin.y) / _worldCellSize.y)
        );
        return coord;
    }

    public int WorldCoordToGridIndex(Vector3 worldCoord)
    {
        var coord = WorldCoordToGridCoord(worldCoord);
        return GridCoordToIndex(coord);
    }
    #endregion
    
    #region Grid room helpers
    public AGridContent GetGridContent(int index)
    {
        if (!_content.ContainsKey(index))
        {
            return null;
        }

        return _content[index];
    }
    
    public void RemoveGridContent(int index)
    {
        //TODO: Salvo la reference per un futuro Destroy() o SetActive(false)
        //AGridContent gridContent = _content[index]; 
        _content.Remove(index);
        OnChanged?.Invoke(this);
    }

    public void AddContent(int index, AGridContent content)
    {
        _content.Add(index, content);
        OnChanged?.Invoke(this);
    }
        
    public Vector2Int GetGridCurrentSize()
    {
        return _currentSize;
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
        OnChanged?.Invoke(this);
    }
    #endregion
    
    #region Grid pathfinding helpers
    public int[] GetAvailableGridIndices()
    {
        var availableIndices = new List<int>();
        for (var x = 0; x < _currentSize.x; x++)
        {
            for (var y = 0; y < _currentSize.y; y++)
            {
                var index = GridCoordToIndex(new Vector2Int(x, y));
                availableIndices.Add(index);
            }
        }

        return availableIndices.ToArray();
    }

    public int[] GetNeighborsIndices(int index)
    {
        var coord = IndexToGridCoord(index);
        var neighbors = new List<int>();
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue; // Skip the current cell
                }

                var neighborCoord = new Vector2(coord.x + x, coord.y + y);

                // Check if neighbor is within bounds
                if (!(neighborCoord.x >= 0) || !(neighborCoord.x < _maxSize.x) ||
                    !(neighborCoord.y >= 0) || !(neighborCoord.y < _maxSize.y))
                {
                    continue;
                }
                var neighborIndex = GridCoordToIndex(new Vector2Int((int)neighborCoord.x, (int)neighborCoord.y));
                neighbors.Add(neighborIndex);
            }
        }

        return neighbors.ToArray();
    }
    #endregion
}