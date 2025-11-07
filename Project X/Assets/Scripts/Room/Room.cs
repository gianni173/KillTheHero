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

    public RoomContent RoomContent
    {
        get => _roomContent;
        set => _roomContent = value;
    }
    
    [SerializeField] 
    private SpriteRenderer _bgRenderer;
    
    private Color _originalColor;
    private Color _newColor = Color.red;

    private Camera _camera;
    
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
    private void Start()
    {
        _originalColor = _bgRenderer.color;
        
        _camera = Camera.main;
    }
    public void Init(RoomData roomData)
    {
        Data = roomData;
        _roomContent.Init(roomData.Contents.IsNullOrEmpty() ? null : roomData.Contents[0]);
        RoomContentDraggableSystem.Instance.RegisterDraggable(_roomContent);
        UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        _bgRenderer.enabled = Data != null;
    }
    
    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        _bgRenderer.color = _newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _bgRenderer.color = _originalColor;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) return;
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

    private void OnDestroy()
    {
        RoomContentDraggableSystem.Instance.UnregisterDraggable(_roomContent);
    }

    public ARoomContentData[] GetRoomContent()
    {
        return Data.Contents;
    }
}