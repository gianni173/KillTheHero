using UnityEngine;
using UnityEngine.UI;

public class ModGridButtons : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] 
    private Button _applyChanges;
    
    [SerializeField] 
    private Button _addWidth;
    
    [SerializeField] 
    private Button _addHeight;
    
    [SerializeField]
    private RoomBuilder _roomBuilder;
    
    private void Awake()
    {
        _roomBuilder = FindFirstObjectByType<RoomBuilder>();
        _applyChanges?.onClick.AddListener(OnApplyChanges);
        _addWidth?.onClick.AddListener(AddWidth);
        _addHeight?.onClick.AddListener(AddHeight);

    }
    
    private void OnApplyChanges()
    {
        _roomBuilder.DestroyRooms();
        _roomBuilder.DestroyContents();
        _roomBuilder.BuildRooms();
        _roomBuilder.BuildContents();
    }

    private void AddWidth()
    {
        // logica da scrivere
    }

    private void AddHeight()
    {
        // logica da scrivere
    }
}
