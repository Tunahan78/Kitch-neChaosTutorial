using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [SerializeField] private PlayerMovement _playermovement;
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playermovement.OnWalkingChance += PlayerMovement_OnWalkingChance;
    }

    private void OnDisable()
    {
        _playermovement.OnWalkingChance -= PlayerMovement_OnWalkingChance;
    }

    private void PlayerMovement_OnWalkingChance(bool isWalking)
    {
        
         _animator.SetBool("IsWalking", isWalking);
        
    }
}
