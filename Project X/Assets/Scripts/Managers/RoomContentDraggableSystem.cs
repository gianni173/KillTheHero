using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class RoomContentDraggableSystem : SerializedMonoBehaviour
{
    public static RoomContentDraggableSystem Instance;
    [OdinSerialize] [Header("Draggable Contents")] 
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
    }

    private void Start()
    {
        _camera = Camera.main;
        if (_camera == null)
            _camera = FindAnyObjectByType<Camera>();
    }
    #region Event registration and unregistration
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
    #endregion
    #region event handlers

    private void OnDraggablePickup(IDraggable invokedDraggable)
    {
        _currentDraggedItem = invokedDraggable;
        if (_currentDraggedItem == null) return;
        _currentDraggedItem.IsDragging = true;

        // Get dragged object's transform
        Transform draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;

        _originalPosition = draggedTransform.position;
        
        // mouse follow 
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        draggedTransform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, _originalPosition.z);
    }

    private void OnDraggableDrag(IDraggable invokedDraggable, PointerEventData eventData)
    {
        if (_currentDraggedItem is not { IsDragging: true }) return;

        var draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;

        var mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
        mouseWorldPos.z = _originalPosition.z;
        // Jachy Hu 07/11: same stuff as RoomDraggableSystem
        var newPosition = new Vector3(mouseWorldPos.x - 0.5f, mouseWorldPos.y - 0.5f, _originalPosition.z);
        draggedTransform.position = newPosition;
    }

    private void OnDraggableRelease(IDraggable invokedDraggable)
    {
        if (_currentDraggedItem == null) return;
        var draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;

        // Memorize room's before moving grid position
        var originalIndex = GridManager.Grid.WorldCoordToGridIndex(_originalPosition);

        // Jachy Hu 07/11: same stuff as RoomDraggableSystem
        var roomPosition = new Vector3(draggedTransform.position.x + 0.5f, draggedTransform.position.y + 0.5f,
            draggedTransform.position.z);
        // Check if dragged room is released in the current visible grid
        var roomGridPosition = GridManager.Grid.WorldCoordToGridCoord(roomPosition);
        var roomIndex = GridManager.Grid.GridCoordToIndex(roomGridPosition);
        var isWithinCurrentSize = roomGridPosition.x >= 0 &&
                                  roomGridPosition.x < GridManager.Grid.CurrentSize.x &&
                                  roomGridPosition.y >= 0 &&
                                  roomGridPosition.y < GridManager.Grid.CurrentSize.y;
        var startingRoomIndex = _gridManager.Grid.WorldCoordToGridIndex(_originalPosition);
        var startingRoomData = _gridManager.Grid.GetGridContent(startingRoomIndex) as RoomData;

        if (!isWithinCurrentSize)
        {
            if (startingRoomData == null)
            {
                // moving from inventory to outside grid check
                draggedTransform.position = _originalPosition;
                return;
            }
            // moved roomContent from grid to inventory
            PlayerStats.Instance.PlayerInventory.AddItemToInventory(startingRoomData.Contents[0]);
            startingRoomData.Contents = Array.Empty<ARoomContentData>();
            _gridManager.Grid.TriggerChange();
            return;
        }
        // Check if there is an existing room   
        var destinationRoomData = _gridManager.Grid.GetGridContent(roomIndex) as RoomData;
        if (destinationRoomData == null)
        {
            draggedTransform.position = _originalPosition;
            return;
        }
        Debug.Log(_currentDraggedItem);
        var monobehaviour = GetTransformFromDraggedRoom(_currentDraggedItem);
        var currentDraggedData = monobehaviour.GetComponent<RoomContent>();
        if (destinationRoomData.Contents.Length == 0)
        {
            destinationRoomData.Contents = new ARoomContentData[1];
            destinationRoomData.Contents[0] = currentDraggedData.GetRoomContentData();  
            if (startingRoomData != null)
            {
                startingRoomData.Contents = Array.Empty<ARoomContentData>();
            }
            else
            {
                //remove roomContent from inventory and place it on grid
                PlayerStats.Instance.PlayerInventory.RemoveItemFromInventory(currentDraggedData.GetRoomContentData());
            }
        }
        else
        {
            //swap logic
            (destinationRoomData.Contents[0], startingRoomData.Contents[0]) = (startingRoomData.Contents[0], destinationRoomData.Contents[0]);
        }
        
        RoomsBuilder.Instance.Refresh();
        draggedTransform.position = GridManager.Grid.GridCoordToWorldCoord(GridManager.Grid.IndexToGridCoord(roomIndex));
        
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
}
