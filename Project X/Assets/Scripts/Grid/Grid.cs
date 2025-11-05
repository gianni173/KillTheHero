using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.VisualScripting.Dependencies.NCalc;
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

    public void MoveRoom(int fromIndex, int toIndex, AGridContent content)
    {
        
        // Remove old room data and connections
        var oldNeighbors = GetNeighborsIndices(fromIndex);
        _content.Remove(fromIndex);
        _connections.Remove(fromIndex);
        // update room with new data's position 
        if (content != null)
        {
            _content[toIndex] = content;
            ConnectNeighbors(toIndex);
        }
        
        foreach (var oldNeighborIndex in oldNeighbors)
        {
            if (_content.ContainsKey(oldNeighborIndex))
            {
                ConnectNeighbors(oldNeighborIndex);
            }
        }
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

        // If all is good, set the new player visible grid
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

        // only look for cardinal direction, no diagonals
        Vector2Int[] directions = {
            new(-1, 0),  // Left
            new(1, 0),   // Right
            new(0, 1),   // Up
            new(0, -1)   // Down
        };

        foreach (var direction in directions)
        {
            var neighborCoord = new Vector2Int(coord.x + direction.x, coord.y + direction.y);

            // Check if neighbor is within bounds
            if (!(neighborCoord.x >= 0) || !(neighborCoord.x < _maxSize.x) ||
                !(neighborCoord.y >= 0) || !(neighborCoord.y < _maxSize.y) ||
                !_content.ContainsKey(GridCoordToIndex(new Vector2Int(neighborCoord.x, neighborCoord.y))))
            {
                continue;
            }

            var neighborIndex = GridCoordToIndex(neighborCoord);
            neighbors.Add(neighborIndex);

        }

        return neighbors.ToArray();
    }

    #endregion

    // toggle connection between two grid indices
    public void ToggleConnection(int fromIndex, int toIndex)
    {
        // if(!CheckConnectionIsValid(fromIndex, toIndex)) return;
        // get current neighbors
        var neighbors = new List<int>();
        if (_connections.ContainsKey(fromIndex))
        {
            neighbors = new List<int>(_connections[fromIndex]);
        }
        // get reverse neighbors
        var reverseNeighbors = new List<int>();
        if (_connections.ContainsKey(toIndex))
        {
            reverseNeighbors = new List<int>(_connections[toIndex]);
        }
        // toggle connection
        if (neighbors.Contains(toIndex))
        {
            neighbors.Remove(toIndex);
            reverseNeighbors.Remove(fromIndex);
        }
        else
        {
            neighbors.Add(toIndex);
            reverseNeighbors.Add(fromIndex);
        }
        _connections[fromIndex] = neighbors.ToArray();
        _connections[toIndex] = reverseNeighbors.ToArray();
        OnChanged?.Invoke(this);
        Debug.Log($"Toggled connection between {fromIndex} and {toIndex}");
    }

    public void AddConnection(int fromIndex, int toIndex)
    {
        // if(!CheckConnectionIsValid(fromIndex, toIndex)) return;
        // get current neighbors
        var neighbors = new List<int>();
        if (_connections.ContainsKey(fromIndex))
        {
            neighbors = new List<int>(_connections[fromIndex]);
        }
        // get reverse neighbors
        var reverseNeighbors = new List<int>();
        if (_connections.ContainsKey(toIndex))
        {
            reverseNeighbors = new List<int>(_connections[toIndex]);
        }
        // add connection
        if (!neighbors.Contains(toIndex))
        {
            neighbors.Add(toIndex);
        }
        if (!reverseNeighbors.Contains(fromIndex))
        {
            reverseNeighbors.Add(fromIndex);
        }
        _connections[fromIndex] = neighbors.ToArray();
        _connections[toIndex] = reverseNeighbors.ToArray();
        OnChanged?.Invoke(this);
    }

    public void RemoveConnection(int fromIndex, int toIndex)
    {
        // if(!CheckConnectionIsValid(fromIndex, toIndex)) return;
        // get current neighbors
        var neighbors = new List<int>();
        if (_connections.ContainsKey(fromIndex))
        {
            neighbors = new List<int>(_connections[fromIndex]);
        }
        // get reverse neighbors
        var reverseNeighbors = new List<int>();
        if (_connections.ContainsKey(toIndex))
        {
            reverseNeighbors = new List<int>(_connections[toIndex]);
        }
        // add connection
        if (neighbors.Contains(toIndex))
        {
            neighbors.Remove(toIndex);
        }
        if (reverseNeighbors.Contains(fromIndex))
        {
            reverseNeighbors.Remove(fromIndex);
        }
        _connections[fromIndex] = neighbors.ToArray();
        _connections[toIndex] = reverseNeighbors.ToArray();
        OnChanged?.Invoke(this);
    }

    // connect all available neighboring grid indices
    public void ConnectAllNeighbors()
    {
        var availableIndices = GetAvailableGridIndices();
        foreach (var index in availableIndices)
        {
            ConnectNeighbors(index);
        }

        OnChanged?.Invoke(this);
    }

    public void ConnectNeighbors(int index)
    {
        var neighbors = GetNeighborsIndices(index);
        _connections[index] = neighbors;
        foreach (var neighborIndex in neighbors)
        {
            AddConnection(neighborIndex, index);
        }
    }
    
    public bool CheckConnectionIsValid(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex){
            return false;
        }
        if (!_content.ContainsKey(fromIndex) || !_content.ContainsKey(toIndex)) {
            return false;
        }
        if (GetGridContent(fromIndex) == null || GetGridContent(toIndex) == null) {
            return false;
        }

        return true;
    }
}