using UnityEngine;
using UnityEngine.EventSystems;

public interface IDraggable : IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging { get; }
    public bool IsDraggable { get; set; }
    public void RegisterToSystem();
    public void UnregisterFromSystem();
    public bool IsValidDropPosition(Vector3 position);
}