using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grid
{
    [Header("Grid Dimensions")]
    [SerializeField] private Vector2Int _maxSize;     // max grid dimension
    [SerializeField] private Vector2Int _currentSize;   // current grid dimension visible to the player
    
    [Header("World Settings")]
    [SerializeField] private Vector3 _origin = Vector3.zero;
    [SerializeField] private Vector2 _worldCellSize = Vector2.one;
    
    
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
        List<int> availableIndices = new List<int>();
        for(int x = 0; x < _currentSize.x; x++)
        {
            for(int y = 0; y < _currentSize.y; y++)
            {
                int index = GridCoordToIndex(new Vector2Int(x, y));
                if(!_content.ContainsKey(index))
                {
                    availableIndices.Add(index);
                }
            }
        }
        

        return availableIndices.ToArray();
    }

}

