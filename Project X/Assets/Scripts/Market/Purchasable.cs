using UnityEngine;

[CreateAssetMenu(fileName = "Purchasable", menuName = "Scriptable Objects/Purchasable")]
public class Purchasable : ScriptableObject
{
    public Sprite Sprite;
    public int MaxPurchases;
    public int Price;
    public int FameNeeded;

    public void Purchase()
    { 
        //logic
    }
}
