using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
     
    public static GameInput Instance { get; private set; }
    public event Action OnInteractAction;
    public event Action OnTakeAction;
    public event Action OnCuttingAction;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("GameInput: Birden fazla instance var!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteractAction?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            OnTakeAction?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            OnCuttingAction?.Invoke();
        }
    }
}
