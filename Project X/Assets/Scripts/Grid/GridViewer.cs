using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.Mathematics;
using UnityEngine;

public class GridViewer<T> : MonoBehaviour where T : MonoBehaviour 
{
    [OdinSerialize]
    private List<T> _content = new List<T>();
    
    public Vector2Int Size;

    public Vector3 Offset = Vector3.zero;

    private void Awake()
    {
        if(_content != null)
             BuildGrid(_content);
    }
    public void BuildGrid(List<T> content)
    {
        for (var i = 0; i < content.Count; i++)
        {
            
        }
    }
}
