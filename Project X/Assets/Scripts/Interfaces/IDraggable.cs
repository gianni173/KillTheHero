using UnityEngine;
using UnityEngine.EventSystems;

public interface IDraggable : IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool IsDragging { get; }
    bool IsDraggable { get; set; }
    void RegisterToSystem();
    void UnregisterFromSystem();
    bool IsValidDropPosition(Vector3 position);
}