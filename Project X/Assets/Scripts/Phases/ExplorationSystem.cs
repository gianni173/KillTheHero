using System.Collections.Generic;
using UnityEngine;

public class ExplorationSystem : Singleton<ExplorationSystem>
{
    [SerializeField]
    private Vector2Int _heroToSpawn = Vector2Int.zero;
    [SerializeField]
    private GameObject _heroPrefab = null;

    private int _entranceRoomIndex = -1;
    private int _mimikRoomIndex = -1;

    private List<GameObject> _heroInstances = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhaseManager.Instance.OnPhaseChanged += StartExploration;
    }

    // Update is called once per frame
    void Update()
    {
        // NextStep();
    }

    public void StartExploration(PhaseType newPhase)
    {
        if (newPhase != PhaseType.Exploration)
            return;

        GetRoomIndices();
        int heroNumber = Random.Range(_heroToSpawn.x, _heroToSpawn.y + 1);

        for (int i = 0; i < heroNumber; i++)
        {
            _heroInstances.Add(Instantiate(_heroPrefab));
        }
    }

    public Room FindSpecificRoom<T>() where T : ARoomContentData
    {
        List<Room> rooms = RoomsBuilder.Instance.Rooms;
        foreach (var room in rooms)
        {
            var contentDatas = room.GetRoomContent();
            foreach (var content in contentDatas)
            {
                if (content != null && content is T)
                {
                    return room;
                }
            }
        }

        return null;
    }
    
    private void GetRoomIndices()
    {
        var mimik = FindSpecificRoom<Mimik>();
        var entrance = FindSpecificRoom<Entrance>();

        if (mimik == null || entrance == null)
        {
            Debug.LogError("Mimik or Entrance room not found!");
            return;
        }
        var grid = GridManager.Instance.Grid;
        _mimikRoomIndex = grid.WorldCoordToGridIndex(mimik.transform.position);
        _entranceRoomIndex = grid.WorldCoordToGridIndex(entrance.transform.position);
    }
}
