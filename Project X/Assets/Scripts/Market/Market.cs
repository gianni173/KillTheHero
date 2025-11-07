using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    [Header("Market Settings")]
    [SerializeField]
    private Button _toggleMarket;

    private bool _isMarketToggled;
    [SerializeField] 
    public Purchasable[] Items;
    public MarketSlot MarketSlotPrefab;
    public GameObject Container;
    
    private void Awake()
    {
        foreach (Purchasable item in Items)
        {
            MarketSlot marketSlot = Instantiate(MarketSlotPrefab, Container.transform);
            marketSlot.Initialize(item , this);
        }
        _toggleMarket?.onClick.AddListener(ActivateMarket);
    }
    private void ActivateMarket()
    {
        var canvasGroup = GetComponentInParent<CanvasGroup>();
        if (!_isMarketToggled)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            _isMarketToggled = true;
            return;
        }
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        _isMarketToggled = false;
    }
    //"return false" for now, waiting until PlayerData is completed.
    public bool CanPurchase(Purchasable item)
    {
        if (item.Price > PlayerStats.Instance.GetResourceQuantity(ResourceType.Gold))
        {
            return false;
        }
        if (item.FameNeeded > PlayerStats.Instance.GetResourceQuantity(ResourceType.Fame))
        {
            return false;
        }
        if (PlayerStats.Instance.GetPurchasedItems(item) >= item.MaxPurchases)
        {
            return false;
        }
        
        return true;
    }

    public bool TryPurchase(Purchasable item)
    {
        if (!CanPurchase(item))
        {
            return false;
        }
        
        item.Purchase();
        return true;
    }
}
