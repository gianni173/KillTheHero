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

    public Action<Vector3> OnPickupProperty { get => OnPickup; set => OnPickup = value; }
    public Action<Vector3> OnReleaseProperty { get => OnRelease; set => OnRelease = value; }
    public bool IsDragging { get; private set; }
    [SerializeField] 
    private bool _isDraggable = true;

    [Header("Debug Feedback")] 
    [SerializeField]
    private bool _enableDebugFeedback = true;
    [SerializeField]
    private Color _normalColor = Color.white;
    [SerializeField] 
    private Color _hoverColor = Color.yellow;
    [SerializeField] 
    private Color _dragColor = Color.green;
    
    //TODO: implementare gli eventi e logica interfacce dentro RoomDraggableSystem
    public Action<Vector3> OnPickup;
    public Action<Vector3> OnRelease;

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

    private void Start()
    {
        RegisterToSystem();
    }

    private void OnDestroy()
    {
        UnregisterFromSystem();
    }

    private void Init(RoomData roomData)
    {
        Data = new RoomData();
        Data = roomData;
    }

    #region IDraggable Implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsDraggable) return;
        
        if (_spriteRenderer != null && _enableDebugFeedback)
        {
            _spriteRenderer.color = _hoverColor;
            Debug.Log("Mouse entered in room");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsDraggable) return;

        if (!IsDragging && _spriteRenderer != null && _enableDebugFeedback)
        {
            _spriteRenderer.color = _normalColor;
            Debug.Log("Mouse exit from room");
        }

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
        
        if (_spriteRenderer != null && _enableDebugFeedback)
        {
            _spriteRenderer.color = _dragColor;
            Debug.Log("room drag started");
        }
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
        
        if (_spriteRenderer != null  && _enableDebugFeedback)
            _spriteRenderer.color = _normalColor;

        Vector3 dropPosition = transform.position;
        
        if (IsValidDropPosition(dropPosition))
        {
            if (_enableDebugFeedback)
                Debug.Log("room position valid");
        }
        else
        {
            transform.position = _originalPosition;
            if (_enableDebugFeedback)
                Debug.Log("room position invalid, reset to original position");
        }

        // OnRelease event invoke
        OnRelease?.Invoke(dropPosition);
        if (_enableDebugFeedback)
            Debug.Log("room drag ended");
    
    }

    public void RegisterToSystem()
    {
        if (_draggableSystem == null)
            _draggableSystem = FindAnyObjectByType<RoomDraggableSystem>();

        //if (_draggableSystem != null)
            //_draggableSystem.RegisterDraggable(this);
    }

    public void UnregisterFromSystem()
    {
        //if (_draggableSystem != null)
           //_draggableSystem.UnregisterDraggable(this);
    }

    public bool IsValidDropPosition(Vector3 position)
    {
        return false;
    }

    #endregion
}
