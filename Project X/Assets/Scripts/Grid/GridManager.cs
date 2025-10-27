using System;
using System.Net.Http.Headers;
using UnityEngine;

public  class GridManager : MonoBehaviour
{
    private Grid _grid;
    public static GridManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    

    public Grid GetGrid()
    {
        return _grid;
    }
}
