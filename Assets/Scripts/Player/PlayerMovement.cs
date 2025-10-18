
using System;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public event Action<bool> OnWalkingChance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerRadius = 0.7f; // Yarıçap
    [SerializeField] private float playerHeight = 2f;  // Boyu
    private bool isWalking;

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, 0f, z).normalized;
        float moveDistance = moveSpeed * Time.deltaTime; 

         // Fizik testi yapıyoruz: Gideceğimiz yönde bir engel var mı?
        bool canMove = !Physics.CapsuleCast( transform.position, transform.position + Vector3.up * playerHeight,  playerRadius,moveDir,moveDistance);

        if (!canMove)
        {
            Vector3 moveDirx = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDirx.magnitude > 0.01f && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirx, moveDistance);

            if (canMove)
            {
                moveDir = moveDirx;
            }
            else
            {

                Vector3 moveDirz = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDirz.magnitude > 0.01f && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirz, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirz;
                }
                else
                {
                    // hiç bir şekilde hareket etmiyor
                }
            }
        }

        // EĞER ÇARPIŞMA YOKSA, hareket et.
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        if (moveDir != Vector3.zero)
        {
            HandleRotation(moveDir);
        }

        bool newIsWalking = moveDir.magnitude > 0;

        if (newIsWalking != isWalking)
        {
            isWalking = newIsWalking;
            OnWalkingChance?.Invoke(isWalking);
        }
    }

    private void HandleRotation(Vector3 moveDir)
    {
        Quaternion targetrotation = Quaternion.LookRotation(moveDir);

        float rotationSpeed = 10f;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);
    }

    private void Update()
    {
        HandleMovement();
    }

}
