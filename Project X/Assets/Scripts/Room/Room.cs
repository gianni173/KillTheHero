using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class Room : SerializedMonoBehaviour, IDraggable
{
    [Header("Room Configuration")]
    [OdinSerialize]
    private RoomData Data { get; set; }

    [SerializeField] 
    private bool _isDraggable = true;
    public bool IsDragging { get; private set; }
    public bool IsDraggable
    {
        get => _isDraggable;
        set => _isDraggable = value;
    }
    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
    public void OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
    }
    }
    #endregion
}
