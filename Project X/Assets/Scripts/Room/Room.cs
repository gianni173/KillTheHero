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

    public Action<Vector3> OnPickupProperty { get; set; }
    public Action<Vector3, PointerEventData> OnDragProperty { get; set; }
    public Action<Vector3> OnReleaseProperty { get; set; }

    public bool IsDragging { get; set; }
    
    [SerializeField] 
    private bool _isDraggable = true;
    public bool IsDraggable
    {
        get => _isDraggable;
        set => _isDraggable = value;
    }

    private void Awake()
    {
        Init(Data);
    }

    private void Init(RoomData roomData)
    {
        Data = roomData ?? new RoomData();
    }

    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        // maybe when mouse enters the tile becomes highlighted?
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // revert onPointerEnter highlight
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        OnPickupProperty?.Invoke(transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable || !IsDragging) return;
        OnDragProperty?.Invoke(transform.position, eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        OnReleaseProperty?.Invoke(transform.position);

    }
    #endregion
}