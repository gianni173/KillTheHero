using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;

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

    private int _indexMovingHero = 0;

    Coroutine _activateHeroCoroutine;

    [SerializeField]
    private float _timeBetweenHeroMoves = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhaseManager.Instance.OnPhaseChanged += StartExploration;
        Entity.OnDeath += OnHeroDeath;
        _grid = GridManager.Instance.Grid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void MoveHero()
    {
        var movingHero = _heroInstances[_indexMovingHero];
        var pathFinderSystem = movingHero.GetComponent<PathFinderSystem>();
        var indexToGrid = pathFinderSystem.NextStep();
        var entity = movingHero.GetComponent<Entity>();
        var roomData = _grid.GetGridContent(indexToGrid) as RoomData;
        foreach(var content in roomData.Contents)
        {
            if(content != null && content.IsUsable)
            {
                content.Interact(entity);
            }
        }
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
            pathFinderSystem.CurrentIndex = _entranceRoomIndex;
            pathFinderSystem.TargetIndex = _mimikRoomIndex;
            pathFinderSystem.SetPath(_pathToMimik);
            hero.SetActive(false); // will be activated when the phase starts
            _heroInstances.Add(hero);
        }
        _activateHeroCoroutine = StartCoroutine(HeroMovements(_timeBetweenHeroMoves));
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
        _mimikRoomIndex = _grid.WorldCoordToGridIndex(mimik.transform.position);
        _entranceRoomIndex = _grid.WorldCoordToGridIndex(entrance.transform.position);

        _pathToMimik = PathFinder.AStarPathFinding(_grid, _entranceRoomIndex, _mimikRoomIndex);
    }

    private void OnHeroDeath(Entity deadHero)
    {
        if (PhaseManager.Instance.CurrentPhase != PhaseType.Exploration)
            return;
        
        _heroInstances[_indexMovingHero].SetActive(false);
        _indexMovingHero++;
        _pathToMimik.ResetPath();
        if (_indexMovingHero >= _heroInstances.Count)
        {
            Debug.Log("All heroes are dead!");
            // End exploration phase
            PhaseManager.Instance.SetPhase(PhaseType.Construction);
            _indexMovingHero = 0;
            if (_activateHeroCoroutine != null)
                StopCoroutine(_activateHeroCoroutine);

            StartCoroutine(RemoveAllHeroes());
        }
    }

    public IEnumerator HeroMovements(float delay)
    {
        while (_indexMovingHero < _heroInstances.Count)
        {
            var hero = _heroInstances[_indexMovingHero];
            if (!hero.activeSelf)
                hero.SetActive(true);
            yield return new WaitForSeconds(delay);
            MoveHero();

        }
        yield return null;
    }

    public IEnumerator RemoveAllHeroes()
    {
        yield return new WaitForSeconds(_timeBetweenHeroMoves);
        foreach (var hero in _heroInstances)
        {
            Destroy(hero);
        }
        _heroInstances.Clear();
        _indexMovingHero = 0;
        if (_activateHeroCoroutine != null)
            StopCoroutine(_activateHeroCoroutine);

        yield return null;
    }
}
