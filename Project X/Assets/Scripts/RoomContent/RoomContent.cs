using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomContent : SerializedMonoBehaviour, IDraggable
{
    public bool IsDragging { get; set; }

    public bool IsDraggable
    {
        get => _isDraggable; 
        set => _isDraggable = value;
    }

    public Action<IDraggable> OnPickupProperty { get; set; }
    public Action<IDraggable, PointerEventData> OnDragProperty { get; set; }
    public Action<IDraggable> OnReleaseProperty { get; set; }
    
    public SpriteRenderer ContentRenderer;

    private ARoomContentData Data { get; set; }

    private Color _originalColor;
    private Color _newColor = Color.red;
    
    [SerializeField]
    private bool _isDraggable = true;
    
    public void Init(ARoomContentData roomContentData)
    {
        Data = roomContentData;
        UpdateGraphics();
    }

    private void Start()
    {
        _originalColor = ContentRenderer.color;
    }
    
    public void UpdateGraphics()
    {
        if (Data == null)
        {
            ContentRenderer.enabled = false;
            return;
        }
 
        ContentRenderer.enabled = true;
        ContentRenderer.sprite = Data.Sprite;
    }

    public ARoomContentData GetRoomContentData()
    {
        return Data;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ContentRenderer.color = _newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ContentRenderer.color = _originalColor;
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
}
