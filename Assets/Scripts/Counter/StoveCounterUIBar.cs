using UnityEngine.UI;
using UnityEngine;
using System;

public class StoveCounterUIBar : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private Image stoveProgressBar;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProcessChanged;
        stoveProgressBar.fillAmount = 0f;
    }

    private void OnDisable()
    {
        stoveCounter.OnProgressChanged -= StoveCounter_OnProcessChanged;
    }

    private void StoveCounter_OnProcessChanged(object sender, StoveCounter.OnProgressChangedEventArgs e)
    {
        stoveProgressBar.fillAmount = e.processNormalizedEvent;
    }
}
