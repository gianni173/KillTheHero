using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Purchasable : SerializedScriptableObject
{
    public Sprite Sprite;
    public int MaxPurchases;
    public int Price;
    public int FameNeeded;

    public virtual bool CanPurchase()
    {
        return true;
    }
    public virtual void Purchase()
    {
        if(!CanPurchase())
            return;
        PlayerStats.Instance.AddResource(ResourceType.Gold , -Price);
        PlayerStats.Instance.AddPurchasableItem(this);
    }
}
