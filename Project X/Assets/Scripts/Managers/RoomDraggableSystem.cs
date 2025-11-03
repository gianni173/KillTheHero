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

    {
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
    

    }//RegisterDraggable

    public void UnregisterDraggable(IDraggable iDraggable)
    {

    }//UnregisterDraggable

}
