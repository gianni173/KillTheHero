using UnityEngine;

public class PurchasableGridExpander : Purchasable
{
    [SerializeField] 
    private Vector2Int _sizeIncrease;
    
    public override void Purchase()
    {
        base.Purchase();

        var currentSize = GridManager.Instance.Grid.GetGridCurrentSize();
        var newSize = new Vector2Int(currentSize.x + _sizeIncrease.x, currentSize.y + _sizeIncrease.y);
        
        GridManager.Instance.Grid.SetCurrentSize(newSize);
    }
}
