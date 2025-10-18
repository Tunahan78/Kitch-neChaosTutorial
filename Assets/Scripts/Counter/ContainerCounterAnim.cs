using System;
using UnityEngine;

public class ContainerCounterAnim : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private CountainerCounter countainerCounter;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        countainerCounter.OnGrabbedObject += PlayerInteraction_OnGrabbedObject;
    }

    private void PlayerInteraction_OnGrabbedObject(object sender, EventArgs e)
    {
        OpenCloseAnim();
    }

    private void OnDisable()
    {
        countainerCounter.OnGrabbedObject -= PlayerInteraction_OnGrabbedObject;
    }

    public void OpenCloseAnim()
    {
        animator.SetTrigger(OPEN_CLOSE);
    }

}
