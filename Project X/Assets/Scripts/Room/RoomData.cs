using UnityEngine;

public class RoomData : AGridContent
{
    public ARoomContentData[] Contents;

    [SerializeField] private int _roomSize = 1;

    public RoomData()
    {
        // 1/11 Jachy Hu: for now it will be initialized as an array with 1 cell
        Contents = new ARoomContentData[_roomSize];
    }
}