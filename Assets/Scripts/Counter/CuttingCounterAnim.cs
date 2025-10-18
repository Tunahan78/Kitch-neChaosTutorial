using System;
using UnityEngine;

public class CuttingCounterAnim : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private CuttingCounter _cuttingCounter;
     private void Awake()
    {
        _cuttingCounter.OnCutting += CuttingCounterAnim_OnCutting;
    }
    private void OnDestroy()
    {
        _cuttingCounter.OnCutting -= CuttingCounterAnim_OnCutting;
    }

    private void CuttingCounterAnim_OnCutting(object sender, EventArgs e)
    {
        _animator.SetTrigger("Cut");
    }
}
