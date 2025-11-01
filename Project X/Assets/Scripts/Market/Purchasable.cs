using UnityEngine;

public abstract class Purchasable : ScriptableObject
{
    public Sprite Sprite;
    public int MaxPurchases;
    public int Price;
    public int FameNeeded;
    
    public virtual void Purchase()
    {
        PlayerStats.Instance.AddResource(ResourceType.Gold , -Price);
        PlayerStats.Instance.AddPurchasableItem(this);
    }
}
