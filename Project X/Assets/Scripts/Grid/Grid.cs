using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid
{
    [SerializeField] private Vector2Int _maxSize;
    [SerializeField] private Vector2Int _currentSize;
    [SerializeField] private Vector3 _origin;
    [SerializeField] private Vector2 _worldCellSize;
    private Dictionary<int, AGridContent> _content = new Dictionary<int, AGridContent>();
    private Dictionary<int, int[]> _connections = new Dictionary<int, int[]>();
    [SerializeField] private Vector2Int _availableTiles;


    public static Vector2 IndexToGridCoord(int index)
    {
        int X = index % GridManager.Instance.GetGrid()._maxSize.x;
        int Y = Mathf.FloorToInt(index / GridManager.Instance.GetGrid()._maxSize.x);

        return new Vector2(X, Y);
    }

    public static int GridCoordToIndex(Vector2Int coord)
    {
        return coord.y * GridManager.Instance.GetGrid()._maxSize.x + coord.x;
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

