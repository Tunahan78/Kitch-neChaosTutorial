// ClearCounter.cs

using UnityEngine;

// IInteractable: Oyuncu buna bakıp etkileşime girebilir.
// IKitchenObjectParent: Bu tezgah bir mutfak objesini tutabilir.
public class ClearCounter : MonoBehaviour, IInteractable, IKitchenObjectParent
{
    // === Serialized Değişkenler (Editor'dan Atananlar) ===
    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private Transform counterTopPoint;       // Objenin üstte duracağı nokta

    // === Private Değişkenler ===
    private KitchenObject kitchenObject; // Counter'ın şu an tuttuğu obje

    private void Start()
    {
        SetSelected(false);
    }

    // =======================================================
    // IInteractable Sözleşmesi Metotları
    // =======================================================

    // ClearCounter.cs - Interact Metodunun Son Hali
// ClearCounter.cs - Interact Metodunun Yeni Hali

public void Interact(PlayerInteraction playerInteraction)
{
    // Gerekli referansları en başta alalım
    IKitchenObjectParent playerHolder = playerInteraction.GetComponent<IKitchenObjectParent>();
    KitchenObject heldObjectByPlayer = playerHolder.GetKitchenObject();
    KitchenObject objectOnCounter = GetKitchenObject();

    // =================================================================
    // CASE 1: TEZGAH BOŞ
    // =================================================================
    if (!HasKitchenObject())
    {
        // Oyuncunun eli doluysa, objeyi tezgaha bırakır.
        if (playerHolder.HasKitchenObject())
        {
            // BURASI SİZİN ÖNERİNİZ: Oyuncunun elinde ne olduğunu kontrol edebiliriz.
            // ClearCounter için bu ayrım şimdilik gereksiz, çünkü her şeyi kabul eder.
            // Ama mantık buraya konulur.
            
            // ADIM ADIM MANUEL TRANSFER (Player -> Counter)
            
            // 1. Oyuncunun elindeki referansı sil.
            playerHolder.SetKitchenObject(null);
            
            // 2. Tezgâhın referansını doldur.
            SetKitchenObject(heldObjectByPlayer);
            
            // 3. Objenin Parent referansını bu tezgâh olarak güncelle.
            // Bu, ileride DestroySelf() için gereklidir.
            heldObjectByPlayer.SetKitchenObjectParent(this);
            
            // 4. Objenin pozisyonunu manuel olarak ayarla (IŞINLANMAYI ENGELLEYEN KISIM).
            heldObjectByPlayer.transform.parent = GetKitchenObjectFollowTransform();
            heldObjectByPlayer.transform.localPosition = Vector3.zero;
        }
        // Oyuncunun eli boşsa ve tezgah da boşsa, hiçbir şey olmaz.
    }
    // =================================================================
    // CASE 2: TEZGAH DOLU
    // =================================================================
    else
    {
        // Oyuncunun eli boşsa...
        if (!playerHolder.HasKitchenObject())
        {
            // SENARYO C: Tezgâhtaki objeyi oyuncu alır.
            // ADIM ADIM MANUEL TRANSFER (Counter -> Player)

            // 1. Tezgâhın referansını sil.
            SetKitchenObject(null);

            // 2. Oyuncunun elini doldur.
            playerHolder.SetKitchenObject(objectOnCounter);

            // 3. Objenin Parent referansını oyuncu olarak güncelle.
            objectOnCounter.SetKitchenObjectParent(playerHolder);
            
            // 4. Objenin pozisyonunu manuel olarak ayarla.
            objectOnCounter.transform.parent = playerHolder.GetKitchenObjectFollowTransform();
            objectOnCounter.transform.localPosition = Vector3.zero;
        }
        // Oyuncunun eli doluysa (Tabak etkileşimleri)
        else
        {
            // SENARYO A: Oyuncunun elinde TABAK var, tezgâhta MALZEME var.
            if (heldObjectByPlayer is PlateKitchenObject playerPlate)
            {
                if (playerPlate.TryAddIngredient(objectOnCounter))
                {
                    // Başarılı: Tezgâhın referansını temizle.
                    // TryAddIngredient içindeki DestroySelf() bunu zaten yapar ama garanti olsun.
                    SetKitchenObject(null); 
                }
            }
            // SENARYO B: Oyuncunun elinde MALZEME var, tezgâhta TABAK var.
            else if (objectOnCounter is PlateKitchenObject counterPlate)
            {
                if (counterPlate.TryAddIngredient(heldObjectByPlayer))
                {
                    // Başarılı: Oyuncunun elini temizle.
                    playerHolder.SetKitchenObject(null);
                }
            }
        }
    }
}

    
    // Görsel Geri Bildirim Metodu
    public void SetSelected(bool isSelected)
    {
        selectedVisual.SetActive(isSelected);
    }

    // =======================================================
    // IKitchenObjectParent Sözleşmesi Metotları
    // =======================================================

    // Objenin nereye konulacağını söyle.
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    // Objenin referansını kaydet/sil.
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    // Counter'daki objeyi döndür.
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // Counter'da obje var mı?
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}