using UnityEngine;

public class RoomContent : MonoBehaviour
{
    [SerializeField] 
    private SpriteRenderer _contentRenderer;

    private ARoomContentData Data { get; set; }

    public void Init(ARoomContentData roomContentData)
    {
        Data = roomContentData;
        UpdateGraphics();
    }
    
    public void UpdateGraphics()
    {
        if (Data == null)
        {
            _contentRenderer.enabled = false;
            return;
        }
 
        _contentRenderer.enabled = true;
        _contentRenderer.sprite = Data.Sprite;
    }
}
