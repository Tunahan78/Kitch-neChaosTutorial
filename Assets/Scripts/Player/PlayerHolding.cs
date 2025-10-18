using Unity.VisualScripting;
using UnityEngine;

public class PlayerHolding : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private KitchenObject kitchenObject; // elinde tuttuğu obje
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
}
