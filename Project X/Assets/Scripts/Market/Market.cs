using System;
using TMPro;
using UnityEngine;

public class Market : MonoBehaviour
{
    public Purchasable[] Items;
    public MarketSlot MarketSlotPrefab;
    public GameObject Container;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _fameText;
    public static Market Instance { get; private set; }
    
    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
            
        foreach (Purchasable item in Items)
        {
            MarketSlot marketSlot = Instantiate(MarketSlotPrefab, Container.transform);
            marketSlot.Initialize(item , this);
        }
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        _goldText.text = "Gold: " + PlayerStats.Instance.GetResourceQuantity(ResourceType.Gold);
        _fameText.text = "Fame: " + PlayerStats.Instance.GetResourceQuantity(ResourceType.Fame);
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
