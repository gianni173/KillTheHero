using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MarketSlot : MonoBehaviour
{
    public Purchasable SlotData;
    [SerializeField] private TextMeshProUGUI _fameCostText;
    [SerializeField] private TextMeshProUGUI _goldCostText;
    private Market _market; // Sarebbe meglio rendere Market un singleton
    private Button _button;
    private Image _image;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _image = GetComponent<Image>();
        
    }

    public void Initialize(Purchasable item, Market market)
    {
        SlotData = item;
        _market = market;
        _image.sprite = SlotData.Sprite;
        _fameCostText.text = SlotData.FameNeeded.ToString();
        _goldCostText.text = SlotData.Price.ToString();
    }
    
    private void OnClick()
    {
        _market.TryPurchase(SlotData);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }
}
