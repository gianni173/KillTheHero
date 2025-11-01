using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchasableRoom_", menuName = "Scriptable Objects/Purchasable")]
public class PurchasableRoom : Purchasable
{
    //checks if there are any free slots in the grid
    private bool CanPurchase()
    {
        if (GridManager.Instance.Grid.GetAvailableGridIndices().Length == 0)
        {
            return false;
        }
        
        Purchase();
        return true;
    }
    
    public override void Purchase()
    {
        //if there aren't any return immediately
        if (!CanPurchase())
        {
            return;
        }
        
        //if there are choose a random slot on the grid and place it there.
        base.Purchase();
        
        var availableIndices = GridManager.Instance.Grid.GetAvailableGridIndices();
        var chosenIndex = Random.Range(0, availableIndices.Length);
        
        GridManager.Instance.Grid.AddContent(availableIndices[chosenIndex], new RoomData());
    }
}
