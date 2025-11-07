using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PhaseManager : Singleton<PhaseManager>
{
    public Action<PhaseType> OnPhaseChanged;
    public Action OnCallFade;

    [SerializeField] 
    private PhaseType _currentPhase;
    public PhaseType CurrentPhase => _currentPhase;

    [Button]
    public void SetPhase(PhaseType newPhase)
    {
        _currentPhase = newPhase;
        OnPhaseChanged?.Invoke(_currentPhase);
        OnCallFade?.Invoke();
    }
}