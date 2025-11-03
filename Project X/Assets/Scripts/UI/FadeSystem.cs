using System.Collections;
using UnityEngine;

public class FadeSystem : MonoBehaviour
{
    private Coroutine _fadeToBlack;
    [SerializeField]
    private PhaseManager _phaseManager;

    private void OnEnable()
    {
        _phaseManager.OnPhaseChanged += EnterFade;
    }

    private void EnterFade() 
    {
        if (_fadeToBlack == null) 
        {
            _fadeToBlack = StartCoroutine(FadeToBlack());
        }
    }

    private IEnumerator FadeToBlack() 
    {
        yield return null;
    }

}
