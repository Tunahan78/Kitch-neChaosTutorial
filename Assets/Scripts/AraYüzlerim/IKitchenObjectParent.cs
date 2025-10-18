
using UnityEngine;
public interface IKitchenObjectParent 
{
   
    Transform GetKitchenObjectFollowTransform(); // objenin nereye koyulacağını belirler
    void SetKitchenObject(KitchenObject kitchenObject); // objeyi kaydet/ sil
    KitchenObject GetKitchenObject(); // objeyi getir
    bool HasKitchenObject(); // objenin olup olmadığını kontrol et
}
