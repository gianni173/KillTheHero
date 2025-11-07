using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    public PlayerStats PlayerStats;

    private void Start()
    {
        PlayerStats = PlayerStats.Instance;
    }
}
