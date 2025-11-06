using UnityEngine;
using System;
using System.Collections.Generic;

public class ExplorationSystem : Singleton<ExplorationSystem>
{
    [SerializeField]
    private Vector2Int _heroToSpawn = Vector2Int.zero;
    [SerializeField]
    private Vector2Int _heroTagsRange = Vector2Int.zero;
    [SerializeField]
    private GameObject _heroPrefab = null;

    private int _entranceRoomIndex = -1;
    private int _mimikRoomIndex = -1;

    private Path _pathToMimik = null;

    private Grid _grid;

    private List<GameObject> _heroInstances = new List<GameObject>();
    
    private int indexMovingHero = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhaseManager.Instance.OnPhaseChanged += StartExploration;
    }

    // Update is called once per frame
    void Update()
    {
        // GET FIRST HERO
        // SET ACTIVE TRUE
        // PathFinderSystem.NextStep() : index in the grid
        // GRID GET CONTENT;
        // Interact with entity in the room
        // ON DEATH advance list to next hero
    }

    public void MoveHero()
    {
        if (_heroInstances[indexMovingHero] == null)
        {
            indexMovingHero++;
        }
        var movingHero = _heroInstances[indexMovingHero];
        movingHero.SetActive(true);
        var pathFinderSystem = movingHero.GetComponent<PathFinderSystem>();
        var indexToGrid = pathFinderSystem.NextStep();
        
    }
    public void StartExploration(PhaseType newPhase)
    {
        if (newPhase != PhaseType.Exploration)
            return;

        GetRoomIndices();
        int heroNumber = UnityEngine.Random.Range(_heroToSpawn.x, _heroToSpawn.y + 1);

        for (int i = 0; i < heroNumber; i++)
        {
            var hero = Instantiate(_heroPrefab);
            var pathFinderSystem = hero.GetComponent<PathFinderSystem>();
            var entity = hero.GetComponent<Entity>();
            //get all hero tags
            var Tags = new List<EntityTag>(Enum.GetValues(typeof(EntityTag)) as EntityTag[]);
            int tagNumber = UnityEngine.Random.Range(_heroTagsRange.x, _heroTagsRange.y + 1);
            entity.TagMask.Clear();
            for (int j = 0; j < tagNumber; j++)
            {
                if (Tags.Count == 0)
                    break;
                int randomIndex = UnityEngine.Random.Range(0, Tags.Count);
                entity.TagMask.Add(Tags[randomIndex]);
                Tags.RemoveAt(randomIndex);
            }
            pathFinderSystem.CurrentIndex = _entranceRoomIndex;
            pathFinderSystem.TargetIndex = _mimikRoomIndex;
            pathFinderSystem.SetPath(_pathToMimik);
            hero.SetActive(false); // will be activated when the phase starts
            _heroInstances.Add(hero);
        }
        
        MoveHero();
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

        _pathToMimik = PathFinder.AStarPathFinding(grid, _entranceRoomIndex, _mimikRoomIndex);
    }
}
