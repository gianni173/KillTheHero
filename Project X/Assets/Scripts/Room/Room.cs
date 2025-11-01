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
    //TODO: implementare gli eventi e logica interfacce dentro RoomDraggableSystem
    public Action<Vector3> OnPickup;
    public Action<Vector3> OnRelease;
    public bool IsDragging { get; private set; }
    public bool IsDraggable
    {
        get => _isDraggable;
        set => _isDraggable = value;
    }
    private Vector3 _originalPosition;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private RoomDraggableSystem _draggableSystem;
    private Camera _camera;


    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _camera = Camera.main;
        Init (Data);
        
        // check a collider component for mouse interaction
        if (_collider2D == null)
            _collider2D = gameObject.AddComponent<BoxCollider2D>();

    }
    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        
        if (_spriteRenderer != null && _enableDebugFeedback)
            _spriteRenderer.color = _hoverColor;
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsDraggable) return;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;

        IsDragging = true;
        _originalPosition = transform.position;
        
        if (_collider2D != null)
            _collider2D.enabled = false;

        // pickup event invoke
        OnPickup?.Invoke(transform.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDraggable || !IsDragging) return;
        
        Vector3 worldPosition = _camera.ScreenToWorldPoint(eventData.position);
        worldPosition.z = transform.position.z;
        
        transform.position = worldPosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDraggable) return;

        IsDragging = false;
        
        if (_collider2D != null)
            _collider2D.enabled = true;

        Vector3 dropPosition = transform.position;
        
        if (IsValidDropPosition(dropPosition))
        {
            if (_enableDebugFeedback)
        }
        else
        {
            transform.position = _originalPosition;
        }

        // OnRelease event invoke
        OnRelease?.Invoke(dropPosition);
    
    }

    public void RegisterToSystem()
    {
        if (_draggableSystem == null)
            _draggableSystem = FindAnyObjectByType<RoomDraggableSystem>();

        if (_draggableSystem != null)
            _draggableSystem.RegisterDraggable(this);
    }

    public void UnregisterFromSystem()
    {
        if (_draggableSystem != null)
            _draggableSystem.UnregisterDraggable(this);
    }

    public bool IsValidDropPosition(Vector3 position)
    {
        return false;
    }

    #endregion
}
