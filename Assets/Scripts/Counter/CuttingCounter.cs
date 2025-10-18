using System;
using UnityEngine;

public class CuttingCounter : MonoBehaviour, IInteractable, IKitchenObjectParent, ICutting
{
    public event EventHandler OnCutting;
    public event EventHandler<OnProcessChangedEventArgs> OnProcessChanged;

    public class OnProcessChangedEventArgs : EventArgs
    {
        public float processNormalized;
    }

    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private Transform counterTopPoint; // Objenin üstte duracağı nokta
    [SerializeField] private CuttingRecipeSO[] cuttingsRecipeArry; // diziden alıcağımız objeyi buna atıyıcaz

    // === Private Değişkenler ===
    private KitchenObject kitchenObject; // Counter'ın şu an tuttuğu obje
    private int cuttingProcess; // Kesme işlemi ilerleme durumu

    private void Start()
    {
        SetSelected(false);
    }
    // Tezgahda bulunan objeye karşılık gelen kesme tarifini var mı 
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObject)
    {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingsRecipeArry)
        {
            if (cuttingRecipe.input == inputKitchenObject)
            {
                return cuttingRecipe;
            }
        }
        return null;
    }
    // Tezgahda bulunan obje kesilebilir mi tarif varsa kesilebilir
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObject)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObject) != null;
    }

    // IInteractable Sözleşmesi Metotları
    public void SetSelected(bool isSelected)
    {
        selectedVisual.SetActive(isSelected);
    }

public void Interact(PlayerInteraction playerInteraction)
    {

        IKitchenObjectParent playerHolder = playerInteraction.GetComponent<IKitchenObjectParent>();
        // Temel Mantık: Counter (Tezgah) dolu mu, boş mu?

        if (!HasKitchenObject()) // CASE 1: Counter BOŞ
        {
            if (playerHolder.HasKitchenObject())
            {
                // Oyuncunun elinde obje var, counter boş. Objeyi tezgaha koy.
                KitchenObject kitchenObjectToTransfer = playerHolder.GetKitchenObject();
                // Oyuncuya senin elin artık boş dedim.
                playerHolder.SetKitchenObject(null);
                // Tezgaha oyuncunun elindeki obje artık senin objen dedim.
                SetKitchenObject(kitchenObjectToTransfer);
                // Objeye diyorum ki artık senin parent'in ben oldum.
                kitchenObjectToTransfer.SetKitchenObjectParent(this);
                kitchenObjectToTransfer.transform.parent = GetKitchenObjectFollowTransform();
                kitchenObjectToTransfer.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            // CASE 2: Counter DOLU 
            KitchenObject heldObject = playerHolder.GetKitchenObject(); // Oyuncudaki obje
            KitchenObject counterObject = GetKitchenObject(); // masadaki obje

            if (playerHolder.HasKitchenObject() && heldObject is PlateKitchenObject playerPlate) // Oyuncuda tabak var masada malzeme var
            {
                if (playerPlate.TryAddIngredient(counterObject))
                {
                    SetKitchenObject(null);
                    return;
                }
                return;
            }

            else if (playerHolder.HasKitchenObject() && counterObject is PlateKitchenObject counterPlate) // oyuncunun elinde malzeme var masada da tabak var 
            {
                if (counterPlate.TryAddIngredient(heldObject))
                {
                    playerHolder.SetKitchenObject(null);
                    return;
                }
                return;
            }

            else if (!playerHolder.HasKitchenObject()) // Masada obje var oyuncunun eli dolu 
            {
                KitchenObject kitchenObjectToTransfer = GetKitchenObject();
                SetKitchenObject(null);
                playerHolder.SetKitchenObject(kitchenObjectToTransfer);
                kitchenObjectToTransfer.transform.parent = playerHolder.GetKitchenObjectFollowTransform();
                kitchenObjectToTransfer.transform.localPosition = Vector3.zero;
                return;
            }
        }
    }



    // IKitchenObjectParent Sözleşmesi Metotları
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    // ICutting Sözleşmesi Metotları

    public void Cutting()
    {
        if (!HasKitchenObject()) return;
        KitchenObjectSO inputKitchenObjectSO = GetKitchenObject().GetKitchenObjectSO();
        if (HasRecipeWithInput(inputKitchenObjectSO))
        {
            cuttingProcess++;

            // Animasyon için event tetikleniyor.
            OnCutting?.Invoke(this, EventArgs.Empty);

            // UI Bar için event tetikleniyor.
            CuttingRecipeSO cuttingRecipeS0 = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
            float processNormalized = (float)cuttingProcess/cuttingRecipeS0.cuttingProgressMax;
            OnProcessChanged?.Invoke(this, new OnProcessChangedEventArgs { processNormalized = processNormalized });

            // Kesme işlemi tamamlandı mı kontrol et.
            CuttingRecipeSO cuttingRecipe = GetCuttingRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());
            if (cuttingProcess >= cuttingRecipe.cuttingProgressMax)
            {
                
                // Kesme işlemi tamamlandı, yeni objeyi oluştur.
                KitchenObjectSO outputKitchenObjectSO = cuttingRecipe.output;
                // Mevcut objeyi yok et
                kitchenObject.DestroySelf();
                cuttingProcess = 0;
                OnProcessChanged?.Invoke(this, new OnProcessChangedEventArgs { processNormalized = 0f });
                // Yeni objeyi oluştur ve tezgaha yerleştir
                Transform outputTransform = Instantiate(cuttingRecipe.output.prefab);
                // Adım 4c: Yeni objenin Parent'ını (Tutucusunu) atama.
                KitchenObject outputKitchenObject = outputTransform.GetComponent<KitchenObject>();
                // CuttingCounter'ı yeni objenin Parent'ı olarak ata.
                SetKitchenObject(outputKitchenObject);

                // Yeni objenin pozisyonunu tezgahın üzerine (counterTopPoint) hizala.
                outputKitchenObject.transform.parent = GetKitchenObjectFollowTransform();
                outputKitchenObject.transform.localPosition = Vector3.zero;
                
            }
            
        }
    }
}
