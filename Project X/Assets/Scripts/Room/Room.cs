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
    private SpriteRenderer _bgRenderer;
    
    [SerializeField] 
    private SpriteRenderer _contentRenderer;
    
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
        UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        _bgRenderer.enabled = Data != null;
        _contentRenderer.enabled = Data != null;
        if (Data == null)
        {
            return;
        }


        if (Data.Contents.IsNullOrEmpty())
        {
            _contentRenderer.enabled = false;
            return;
        }

        _contentRenderer.enabled = true;
        _contentRenderer.sprite = Data.Contents[0].Sprite;
    }
    
    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO:maybe when mouse enters the tile becomes highlighted?
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO:revert onPointerEnter highlight
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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