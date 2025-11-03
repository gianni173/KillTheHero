using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PhaseManager : Singleton<PhaseManager>
{

    [SerializeField]
    private PhaseType _currentPhase;
    public PhaseType CurrentPhase => _currentPhase;
    public Action<PhaseType> OnPhaseChanged;
    [Button]
    public void SetPhase(PhaseType newPhase)
    {
        _currentPhase = newPhase;
        OnPhaseChanged?.Invoke(_currentPhase);
    }
}
