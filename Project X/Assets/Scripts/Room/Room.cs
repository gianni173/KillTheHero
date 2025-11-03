using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class Room : SerializedMonoBehaviour, IDraggable
{
    [Header("Room Configuration")]
    [OdinSerialize, ReadOnly]
    private RoomData Data { get; set; }

    [SerializeField] 
    private RoomContent _roomContent;
    
    [SerializeField] 
    private SpriteRenderer _bgRenderer;
    
    public Action<IDraggable> OnPickupProperty { get; set; }
    public Action<IDraggable, PointerEventData> OnDragProperty { get; set; }
    public Action<IDraggable> OnReleaseProperty { get; set; }


    public bool IsDragging { get; set; }
    
    [SerializeField] 
    private bool _isDraggable = true;
    public bool IsDraggable
    {
        get => _isDraggable;
        set => _isDraggable = value;
    }

    public void Init(RoomData roomData)
    {
        Data = roomData;
        _roomContent.Init(roomData.Contents.IsNullOrEmpty() ? null : roomData.Contents[0]);
        UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        _bgRenderer.enabled = Data != null;
    }
    
    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO:maybe when mouse enters the tile becomes highlighted?
        Debug.Log("[ROOM] Mouse entered.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO:revert onPointerEnter highlight
        Debug.Log("[ROOM] Mouse exited.");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("[ROOM] Mouse picked up.");
        if (!IsDraggable) return;
        OnPickupProperty?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable || !IsDragging) return;
        OnDragProperty?.Invoke(this, eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        OnReleaseProperty?.Invoke(this);
    }
    #endregion
}