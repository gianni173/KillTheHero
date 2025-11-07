using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class FadeSystem : SerializedMonoBehaviour
{
    private Coroutine _fadeToBlack;
    //[SerializeField]
    //private AnimationCurve _fadeCurve;
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private float _fadeTime = 1f;
    [SerializeField]
    private float _cooldown = 1f;
    private void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        if (_canvasGroup.alpha == 0) 
        {
            SetBlockRaycast(false);
        }
    }
    private void OnEnable()
    {
        PhaseManager.OnCallFade += EnterFade;
    }
    private void OnDisable()
    {
        PhaseManager.OnCallFade -= EnterFade;
    }

    private void EnterFade() 
    {
        if (_fadeToBlack == null)
        {
            _fadeToBlack = StartCoroutine(FadeToBlack());
            SetBlockRaycast(true);
        }
        else 
        {
            Debug.LogWarning("[FADESYSTEM] Wait for coroutine to end!");
        }
    }

    private IEnumerator FadeToBlack() 
    {
        float t = 0;
        while (t < 1) 
        {
            _canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            t += Time.deltaTime / _fadeTime;
            yield return new WaitForEndOfFrame();
        }
        _canvasGroup.alpha = 1;
        yield return new WaitForSeconds(_cooldown);
        StartCoroutine(FadeFromBlack());
    }
    private IEnumerator FadeFromBlack()
    {
        float t = 0;
        while (t < 1)
        {
            _canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime / _fadeTime;
            yield return new WaitForEndOfFrame();
        }
        _canvasGroup.alpha = 0;
        _fadeToBlack = null;
        SetBlockRaycast(false);
    }

    private void SetBlockRaycast(bool toggle)
    {
        _canvasGroup.blocksRaycasts = toggle;
    }
}
