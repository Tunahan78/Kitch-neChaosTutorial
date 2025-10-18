using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : MonoBehaviour , IInteractable
{
    [Header("References")]
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform countertopPoint;
    [SerializeField] private GameObject selectedVisual;

    [Header("Settings")]
    [SerializeField] private float platesSpawnTimerMax;
    [SerializeField] private float platesSpawnAmountMax;
    [SerializeField] private float platesOffsetY;

    private float plateSpawnTimer;
    private float platesSpawnAmount;
    private List<KitchenObject> platesList;

    private void Awake()
    {
        platesList = new List<KitchenObject>();
    }
    private void Start()
    {
        plateSpawnTimer = 0f;
        platesSpawnAmount = 0f;
        selectedVisual.SetActive(false);
    }

    private void Update()
    {
        if(platesSpawnAmount <=platesSpawnAmountMax)
        {
            plateSpawnTimer += Time.deltaTime;
            if(plateSpawnTimer >= platesSpawnTimerMax)
            {
                SpawnPlate();
                plateSpawnTimer = 0f;
            }
        }
    }

    private void SpawnPlate()
    {
        Transform plateTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject plateKitchenObject = plateTransform.GetComponent<KitchenObject>();
        platesList.Add(plateKitchenObject);
        platesSpawnAmount++;

        plateTransform.parent = countertopPoint;
        plateTransform.localPosition = new Vector3(0f, platesOffsetY * (platesSpawnAmount - 1), 0f);
    }

     public void Interact(PlayerInteraction playerInteraction)
    {
        IKitchenObjectParent playerHolder = playerInteraction.GetComponent<IKitchenObjectParent>();
        // oyuncunun eli boşsa
        if (!playerHolder.HasKitchenObject())
        {
            if (platesSpawnAmount > 0)
            {
                KitchenObject plateToGive = platesList[platesList.Count - 1];
                plateToGive.SetKitchenObjectParent(playerHolder);
                platesList.RemoveAt(platesList.Count - 1);
                platesSpawnAmount--;
            }
        }
        // CASE 2: OYUNCUNUN ELİ DOLU
        else
        {
            KitchenObject heldObject = playerHolder.GetKitchenObject();

            // KRİTİK KONTROL: Tezgâhın üzerinde tabak var mı?
            if (platesSpawnAmount > 0)
            {
                // Listenin en üstündeki tabağa eriş
                KitchenObject topPlate = platesList[platesList.Count - 1];

                // ELİMİZDEKİ OBJEYİ TABAĞA YÖNLENDİR
                if (topPlate is PlateKitchenObject plateKitchenObject)
                {
                    // Tabağa malzemeyi eklemeyi dene
                    bool ingredientAdded = plateKitchenObject.TryAddIngredient(heldObject);

                    if (ingredientAdded)
                    {
                        // Malzeme başarıyla eklendi. heldObject (oyuncunun elindeki)
                        // TryAddIngredient metodu içinde zaten yok edildi.
                        Debug.Log($"Malzeme eklendi: {heldObject.GetKitchenObjectSO().sprite.name}");
                    }
                    else
                        Debug.LogError("Malzeme eklenemedi");
                }
            }

        }
    }

    public void SetSelected(bool isSelected)
    {
        selectedVisual.SetActive(isSelected);
    }

    
}
