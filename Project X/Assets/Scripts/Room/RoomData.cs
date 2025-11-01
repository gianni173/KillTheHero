using UnityEngine;
 public class RoomData : AGridContent
 {
    [SerializeField] 
    private int _roomSize = 1;
    public ARoomContentData[] _contents;
    
    public RoomData()
    {
        // 1/11 Jachy Hu: for now it will be initialized as an array with 1 cell
        _contents = new ARoomContentData[_roomSize];
    }

}

