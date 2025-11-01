using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [Header("Prefabs da spawnare")]
    public GameObject tilePrefab;
    public GameObject entrancePrefab;
    public GameObject mimicPrefab;

    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.Instance;

        if (gridManager == null)
        {
            return;
        }
        
        BuildRooms();
    }

    private void BuildRooms()
    {
        Grid grid = gridManager.Grid;

        Vector2Int size = gridManager.Grid.CurrentSize;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int index = grid.GridCoordToIndex(new Vector2Int(x, y));
                Vector3 pos = grid.GridCoordToWorldCoord(new Vector2(x, y));

                GameObject prefabToSpawn = tilePrefab;

                if (index == 0)
                {
                    prefabToSpawn = entrancePrefab;
                }
                
                else if (index == size.x * size.y - 1)
                {
                    prefabToSpawn = mimicPrefab;
                }
                
                Instantiate(prefabToSpawn, pos, Quaternion.identity);
            }
        }
    }
}