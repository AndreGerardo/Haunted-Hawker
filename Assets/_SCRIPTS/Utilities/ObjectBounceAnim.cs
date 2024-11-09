using System;
using UnityEngine;
using DG.Tweening;

public class ObjectBounceAnim : MonoBehaviour
{
    [Header("ANIMATION CONFIGURATION")]
    [SerializeField]
    private float targetScaleFactor = 1.1f;
    [SerializeField]
    private float animDuration = 0.15f;

    private Transform objTransform;
    private Vector3 startingScale;

    private void Start()
    {
        objTransform = transform;
        startingScale = objTransform.localScale;
    }

    public void Play(Action OnAnimComplete = null)
    {
        objTransform.DOScale(targetScaleFactor * startingScale, animDuration)
            .SetEase(Ease.InSine);
        objTransform.DOScale(startingScale, animDuration)
            .SetDelay(animDuration)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                OnAnimComplete?.Invoke();
            });
    }
}
