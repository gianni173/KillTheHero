using UnityEngine;
using Sirenix.OdinInspector;

public class PhaseManager : Singleton<PhaseManager>
{

    [SerializeField]
    private PhaseType _currentPhase;
    public PhaseType CurrentPhase => _currentPhase;
    [Button]
    public void SetPhase(PhaseType newPhase)
    {
        _currentPhase = newPhase;
    }
}
