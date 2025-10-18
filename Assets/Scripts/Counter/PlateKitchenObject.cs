using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    [Serializable]
    public struct KitchenObjectSO_GameObject{
    public KitchenObjectSO kitchenObjectSO; // objenin SO su 
    public GameObject gameObject; // Aktif/Pasif edilecek alt obje
    public GameObject uıIconGameObject; // Aktif/Pasif edilecek obje ikonu
}

    [SerializeField] private List<KitchenObjectSO> validKitchenObjects; // Bu listede var olan KitchenObjectSO'lar tabağa eklenebilir.

    [SerializeField] private List<KitchenObjectSO_GameObject> ingredientGameObjectList;
    private List<KitchenObjectSO> kitchenObjectSOList; // Tabağa eklenen KitchenObject'lerin listesi.

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
        foreach (KitchenObjectSO_GameObject ingredientGameObject in ingredientGameObjectList)
        {
            ingredientGameObject.gameObject.SetActive(false);
        }
    }
    public bool TryAddIngredient(KitchenObject igredientkitchenObject)
    {
        KitchenObjectSO igredientSO = igredientkitchenObject.GetKitchenObjectSO();
        // Eğer ingredient validKitchenObjects listesinde yoksa false döner.
        if (!validKitchenObjects.Contains(igredientSO))
        {
            return false;
        }
        // Eğer ingredient zaten kitchenObjectSOList listesinde varsa false döner.
        if (kitchenObjectSOList.Contains(igredientSO))
        {
            return false;
        }
        kitchenObjectSOList.Add(igredientSO);
        foreach(KitchenObjectSO_GameObject ingredientGameObject in ingredientGameObjectList)
        {
            if (ingredientGameObject.kitchenObjectSO == igredientSO)
            {
                Debug.Log("Aktif etmeli");
                ingredientGameObject.gameObject.SetActive(true);
                
                if (ingredientGameObject.uıIconGameObject != null)
                {
                    ingredientGameObject.uıIconGameObject.SetActive(true);
                }
            
                break;
            }
            
        }
        IKitchenObjectParent oldParent = igredientkitchenObject.GetKitchenObjectParent();
        if (oldParent != null)
        {
            oldParent.SetKitchenObject(null);
        }
        Destroy(igredientkitchenObject.gameObject);
        return true;
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
