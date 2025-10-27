using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Grid
{
    private Vector2Int _maxSize;
    private Vector2Int _currentSize;
    private Vector3 _origin;
    private Vector2 _worldCellSize;
    private Dictionary<int, AGridContent> _content = new Dictionary<int, AGridContent>();
    private Dictionary<int, int[]> _connections = new Dictionary<int, int[]>();
    private Vector2Int _availableTiles;
}

