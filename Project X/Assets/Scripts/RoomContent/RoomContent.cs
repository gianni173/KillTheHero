using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomContent : SerializedMonoBehaviour, IDraggable
{
    [SerializeField] 
    private SpriteRenderer _contentRenderer;
    public SpriteRenderer ContentRenderer;
    
    [OdinSerialize]
    private ARoomContentData Data { get; set; }
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
    

    private Color _originalColor;
    private Color _newColor = Color.red;
    
    public void Init(ARoomContentData roomContentData)
    {
        Data = roomContentData;
        if (roomContentData == null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
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
    #region IDraggable Implementation
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
    #endregion
}
