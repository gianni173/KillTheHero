using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDraggable : IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<IDraggable> OnPickupProperty { get; set; }
    public Action<IDraggable, PointerEventData> OnDragProperty { get; set; }
    public Action<IDraggable> OnReleaseProperty { get; set; }


    
    public bool IsDragging { get; set; }
    public bool IsDraggable { get; set; }
    //public bool IsValidDropPosition(Vector3 position);
}