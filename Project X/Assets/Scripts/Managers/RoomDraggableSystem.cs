using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomDraggableSystem : MonoBehaviour, IDraggable
{
    public List<IDraggable> Draggables;

    public void RegisterDraggable(IDraggable Idraggable)
    {
        
    }
    public void UnregisterDraggable(IDraggable draggable)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    //Edoa: I don't think OnPointerEnter and OnPointerExit are
    //going to be that useful, but I'll keep them for now

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public bool IsDragging { get; }
    public bool IsDraggable { get; set; }
    public void RegisterToSystem()
    {
        throw new System.NotImplementedException();
    }

    public void UnregisterFromSystem()
    {
        throw new System.NotImplementedException();
    }

    public bool IsValidDropPosition(Vector3 position)
    {
        throw new System.NotImplementedException();
    }
}
