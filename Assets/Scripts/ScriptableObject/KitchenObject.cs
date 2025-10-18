using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO _kitchenobject; // objenin türü

    private IKitchenObjectParent _kitchenObjectParent; // objenin parenti

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenobject;
    }
    public void DestroySelf()
    {
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.SetKitchenObject(null);
        }
        Destroy(gameObject);
    }
    public void SetKitchenObjectParent(IKitchenObjectParent newParent)
    {
        //Bu methot eski parentı silmiyor ben eklemeye çalıştım ancak gende çalışmadı aklında bulunsun
        _kitchenObjectParent = newParent;
        if (newParent != null)
        {
            newParent.SetKitchenObject(this);
            transform.parent = newParent.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }

    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }

}
