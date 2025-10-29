using UnityEngine;

public  class GridManager : MonoBehaviour
{
    [SerializeField] private Grid _grid;
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
