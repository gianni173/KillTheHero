using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomContentDraggableSystem : SerializedMonoBehaviour
{
    public static RoomContentDraggableSystem Instance;
    public GridManager GridManager
    {
        get
        {
            _gridManager = GridManager.Instance;
            return _gridManager;
        }
    }
    
    [OdinSerialize] [Header("Draggable Contents")]
    private List<IDraggable> _draggables = new();
    private GridManager _gridManager;
    private Camera _camera;
    private IDraggable _currentDraggedContent;
    private Room _originalRoom;
    private Vector3 _originalPosition;
    
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
        {
            _camera = FindAnyObjectByType<Camera>();
        }
    }

    public void RegisterDraggable(IDraggable draggable)
    {
        if (draggable == null || _draggables.Contains(draggable))
        {
            return;
        }
        
        _draggables.Add(draggable);

        draggable.OnPickupProperty += OnDraggablePickup;
        draggable.OnDragProperty += OnDraggableDrag;
        draggable.OnReleaseProperty += OnDraggableRelease;
    }

    private void OnDraggablePickup(IDraggable draggable)
    {
        _currentDraggedContent = draggable;
        if (_currentDraggedContent == null)
        {
            return;
        }

        _currentDraggedContent.IsDragging = true;
        //save original room
        _originalRoom = GetRoomFromContent(_currentDraggedContent);
        
        //get dragged Transform
        var draggedTransform = GetDraggedContentTransform();
        if (draggedTransform == null)
        {
            return;
        }
        
        _originalPosition = draggedTransform.position;
        
        //mouse follow
        var mouseWorldPos = GetMouseWorldPosition();
        
        //subtract 0.5 from the x and y values to keep in consideration the offset of the room content's position
        draggedTransform.position = new Vector3(mouseWorldPos.x - 0.5f, mouseWorldPos.y - 0.5f, _originalPosition.z);
    }

    private void OnDraggableDrag(IDraggable draggable, PointerEventData eventData)
    {
        if (_currentDraggedContent is not { IsDragging : true })
        {
            return;
        }
        
        var draggedTransform = GetDraggedContentTransform();
        if (draggedTransform == null)
        {
            return;
        }
        
        var mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y));
        mouseWorldPos.z =  _originalPosition.z;
        
        draggedTransform.position = new Vector3(mouseWorldPos.x - 0.5f, mouseWorldPos.y - 0.5f, _originalPosition.z);
        
    }

    private void OnDraggableRelease(IDraggable draggable)
    {
        if (_currentDraggedContent == null)
        {
            return;
        }
        var draggedTransform = GetDraggedContentTransform();
        if (draggedTransform == null)
        {
            return;
        }
        
        //get the index of the room the content is currently on top.
        var roomIndex = GridManager.Grid.WorldCoordToGridIndex(GetMouseWorldPosition());

        if (roomIndex < 0 || roomIndex > (GridManager.Grid.CurrentSize.x * GridManager.Grid.CurrentSize.x))
        {
            draggedTransform.position = _originalPosition;
            return;
        }
        
        //check if the grid has a room placed at the given index
        if (_gridManager.Grid.GetGridContent(roomIndex) == null)
        {
            draggedTransform.position = _originalPosition;
            return;
        }
        
        var destinationRoomData = _gridManager.Grid.GetGridContent(roomIndex) as RoomData;
        var startingRoomIndex = _gridManager.Grid.WorldCoordToGridIndex(_originalRoom.transform.position);
        var startingRoomData = _gridManager.Grid.GetGridContent(startingRoomIndex) as RoomData;
        var currentDraggedData = _currentDraggedContent as RoomContent;

        if (destinationRoomData.Contents.Length == 0)
        {
            destinationRoomData.Contents = new ARoomContentData[1];
            destinationRoomData.Contents[0] = currentDraggedData.GetRoomContentData();
            startingRoomData.Contents = Array.Empty<ARoomContentData>();
        }
        else
        {
            (destinationRoomData.Contents[0], startingRoomData.Contents[0]) = (startingRoomData.Contents[0], destinationRoomData.Contents[0]);
        }
        
        RoomsBuilder.Instance.Refresh();

        draggedTransform.position = GridManager.Grid.GridCoordToWorldCoord(GridManager.Grid.IndexToGridCoord(roomIndex));
        
        _currentDraggedContent.IsDragging = false;
        _currentDraggedContent = null;
    }

    private Room GetRoomFromContent(IDraggable draggable)
    {
        return ((MonoBehaviour)draggable).GetComponentInParent<Room>();
    }

    private Transform GetDraggedContentTransform()
    {
        return ((MonoBehaviour)_currentDraggedContent).transform;
    }
    
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
}
