using UnityEngine;

public class Market : MonoBehaviour
{
    public Purchasable[] Items;
    public PlayerStats PlayerStats;

    //"return false" for now, waiting until PlayerData is completed.
    public bool CanPurchase(Purchasable Item)
    {
        //logic
        return false;
    }

    public bool TryPurchase(Purchasable item)
    {
        //logic
        return false;
    }
}
