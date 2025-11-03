using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
public class RoomDraggableSystem : SerializedMonoBehaviour
{
    [OdinSerialize]
    private List<IDraggable> _draggables = new();
    private IDraggable _currentDraggedItem;
    private Camera _camera;
    private Vector3 _originalPosition;
    private void Awake()
    {
        RegisterAllDraggables();
    }

    private void Start()
    {
        _camera = Camera.main;
        if (_camera == null)
            _camera = FindAnyObjectByType<Camera>();
    }

    private void Update()
    {
        //DEBUG: Call RegisterAllDraggables() on runtime with a key, since it registers draggables before they can be built.
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            RegisterAllDraggables();
        }
    }

    private void RegisterAllDraggables()
    {
        Room[] allRooms = FindObjectsByType<Room>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
    
        foreach (Room room in allRooms)
        {
            RegisterDraggable(room);
            Debug.Log($"Registered Room: {room.name}");
        }
    
        Debug.Log($"Total registered draggables: {_draggables.Count}");
    }
    
    public void RegisterDraggable(IDraggable draggable)
    {
        if (draggable == null || _draggables.Contains(draggable)) return;
        
        _draggables.Add(draggable);
        
        // Sottoscrivi agli eventi
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
        if (_currentDraggedItem == null || !_currentDraggedItem.IsDragging) return;

        Transform draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;
        
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10f));
        mouseWorldPos.z = _originalPosition.z;
        
        Vector3 newPosition = new Vector3(mouseWorldPos.x, mouseWorldPos.y, _originalPosition.z);
        draggedTransform.position = newPosition;
    }

    private void OnDraggableRelease(IDraggable invokedDraggable)
    {
        if (_currentDraggedItem == null) return;
        Transform draggedTransform = GetTransformFromDraggedRoom(_currentDraggedItem);
        if (draggedTransform == null) return;
        
        // TODO: GridManager will decide where to drop this Room
        // for now will just leave the current position
        if (true)
        {
            Vector3 dropPosition = draggedTransform.position;
            draggedTransform.position = dropPosition;
        }
        else
        {
            // reset position
            draggedTransform.position = _originalPosition;
        }
        // reset values
        _currentDraggedItem.IsDragging = false;
        _currentDraggedItem = null;
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
    private Transform GetTransformFromDraggedRoom(IDraggable draggable)
    {
        return ((MonoBehaviour)draggable).transform;
    }

    private bool CheckGridPosition(Vector3 position)
    {
        // TODO: GridManager interaction
        return true;
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