using System;
using UnityEngine;
using UnityEngine.UI;

public class CuttingUIBar : MonoBehaviour
{
    [SerializeField] private Image cuttingProgressBar;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start()
    {
        cuttingCounter.OnProcessChanged += CuttingCounter_OnProcessChanged;
        cuttingProgressBar.fillAmount = 0f;
    }

    private void CuttingCounter_OnProcessChanged(object sender, CuttingCounter.OnProcessChangedEventArgs e)
    {
        Debug.Log("CuttingUIBar: " + e.processNormalized);
        cuttingProgressBar.fillAmount = e.processNormalized;
    }

    private void OnDisable()
    {
        cuttingCounter.OnProcessChanged -= CuttingCounter_OnProcessChanged;
    }

}
