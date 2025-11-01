using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [Header("Prefabs da spawnare")] public GameObject tilePrefab;
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

        BuildContents();
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
                
                Instantiate(tilePrefab, pos, Quaternion.identity);
            }
        }
    }

    private void BuildContents()
    {
        Grid grid = gridManager.Grid;
        Vector2Int size = gridManager.Grid.CurrentSize;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int index = grid.GridCoordToIndex(new Vector2Int(x, y));
                Vector3 pos = grid.GridCoordToWorldCoord(new Vector2(x, y));

                if (index == 0)
                {
                    // Spawn dell’entrata sopra la tile
                    Instantiate(entrancePrefab, pos + Vector3.up * 0.01f, Quaternion.identity);
                }
                else if (index == size.x * size.y - 1)
                {
                    // Spawn del Mimic sopra la tile
                    Instantiate(mimicPrefab, pos + Vector3.up * 0.01f, Quaternion.identity);
                }
            }
        }
    }
}