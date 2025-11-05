using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
public class RoomDraggableSystem : SerializedMonoBehaviour
{
    public static RoomDraggableSystem Instance;
    [OdinSerialize]
    private List<IDraggable> _draggables = new();
    private IDraggable _currentDraggedItem;
    private Camera _camera;
    private Vector3 _originalPosition;
    private GridManager _gridManager;
    public GridManager GridManager
    {
        get
        {
            _gridManager = GridManager.Instance;
            return _gridManager;
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //TODO: move it to Room script
        //RegisterAllDraggables();
    }
    
    private void Start()
    {
        _camera = Camera.main;
        if (_camera == null)
            _camera = FindAnyObjectByType<Camera>();
    }
    public void RegisterDraggable(IDraggable draggable)
    {
        if (draggable == null || _draggables.Contains(draggable)) return;
        
        _draggables.Add(draggable);
        
        // Event subscription
        draggable.OnPickupProperty += OnDraggablePickup;
        draggable.OnDragProperty += OnDraggableDrag;
        draggable.OnReleaseProperty += OnDraggableRelease;
    }

    public void UnregisterDraggable(IDraggable draggable)
    {
        if (draggable == null || !_draggables.Contains(draggable)) return;
        
        _draggables.Remove(draggable);
        
        // Rimuovi sottoscrizione agli eventi
        draggable.OnPickupProperty -= OnDraggablePickup;
        draggable.OnDragProperty -= OnDraggableDrag;
        draggable.OnReleaseProperty -= OnDraggableRelease;
    }
    #region event handlers
    private void OnDraggablePickup(IDraggable invokedDraggable)
    {
        _currentDraggedItem = invokedDraggable;
        if (_currentDraggedItem == null) return;
        _currentDraggedItem.IsDragging = true;
        
        // Ottieni il Transform dell'oggetto
        Transform draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;
        
        _originalPosition = draggedTransform.position;
        
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        draggedTransform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, _originalPosition.z);
    }

    private void OnDraggableDrag(IDraggable invokedDraggable, PointerEventData eventData)
    {
        if (_currentDraggedItem is not { IsDragging: true }) return;

        Transform draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;
        
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
        mouseWorldPos.z = _originalPosition.z;
        // Jachy Hu 05/11: this 0.5f is to take consideration of the room prefab offset in bg renderer (and also the whole grid)
        Vector3 newPosition = new Vector3(mouseWorldPos.x - 0.5f, mouseWorldPos.y - 0.5f, _originalPosition.z);
        draggedTransform.position = newPosition;
    }

    private void OnDraggableRelease(IDraggable invokedDraggable)
{
    if (_currentDraggedItem == null) return;
    Transform draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
    if (draggedTransform == null) return;
    // Calcola la posizione originale nella griglia
    var originalGridPosition = GridManager.Grid.WorldCoordToGridCoord(_originalPosition);
    var originalIndex = GridManager.Grid.GridCoordToIndex(originalGridPosition);

    // Jachy Hu 05/11: same as OnDraggable but reverted to restore the TRUE position values
    // because the dragged object you see on the mouse has that offset into account
    var roomPosition = new Vector3(draggedTransform.position.x + 0.5f, draggedTransform.position.y + 0.5f, draggedTransform.position.z);
        
    // Check if dragged room is released in the current visible grid
    var roomGridPosition = GridManager.Grid.WorldCoordToGridCoord(roomPosition);
    var roomIndex = GridManager.Grid.GridCoordToIndex(roomGridPosition);
    bool isWithinCurrentSize = roomGridPosition.x >= 0 && 
                               roomGridPosition.x < GridManager.Grid.CurrentSize.x &&
                               roomGridPosition.y >= 0 && 
                               roomGridPosition.y < GridManager.Grid.CurrentSize.y;
    
    if (!isWithinCurrentSize)
    {
        // Reset to original position
        Debug.Log("Oggetto fuori dai limiti della griglia - ripristino posizione originale");
        draggedTransform.position = _originalPosition;
        return;
    }

    // Check if there is an existing room
    var existingContent = GridManager.Grid.GetGridContent(roomIndex);
    
    if (existingContent != null)
    {
        // Reset to original position
        Debug.Log("Cella già occupata - ripristino posizione originale");
        draggedTransform.position = _originalPosition;
        return;
    }

    // Put the dragged room into that cell coordinates
    Vector3 gridWorldPosition = GridManager.Grid.GridCoordToWorldCoord(roomGridPosition);
    draggedTransform.position = gridWorldPosition;
    Debug.Log($"Oggetto posizionato nella griglia alla posizione: {roomGridPosition}");
    var roomContent = GridManager.Grid.GetGridContent(originalIndex);
    GridManager.Grid.MoveRoom(originalIndex, roomIndex, roomContent);

    // Reset drag values
    _currentDraggedItem.IsDragging = false;
    _currentDraggedItem = null;
}

    #endregion
    private Vector3 GetMouseWorldPosition()
    {
        if (_camera == null)
        {
            Debug.LogError("Camera Missing - cannot get mouse world position");
            return Vector3.zero;
        }
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 worldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
        return worldPos;
    }
    private Transform GetTransformFromDraggedRoom(IDraggable draggable)
    {
        return ((MonoBehaviour)draggable).transform;
    }
    private void OnDestroy()
    {
        // Cleanup quando il sistema viene distrutto
        foreach (IDraggable draggable in _draggables)
        {
            UnregisterDraggable(draggable);
        }
        _draggables.Clear();
    }
}