
using System;
using UnityEngine;

// IInteractable: Oyuncu buna bakıp etkileşime girebilir.
// IKitchenObjectParent: Bu tezgah bir mutfak objesini tutabilir.
public class CountainerCounter : MonoBehaviour, IInteractable, IKitchenObjectParent
{
    public event EventHandler OnGrabbedObject;
    // === Serialized Değişkenler (Editor'dan Atananlar) ===
    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private KitchenObjectSO kitchenObjectSO; // Hangi objeyi spawn edeceğini belirleyen veri
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

    // E Tuşuna Basıldığında Çağrılır (Oyuncunun Genel Etkileşim Aksiyonu)
    public void Interact(PlayerInteraction playerInteraction)
    {
        // Temel Mantık: Counter (Tezgah) dolu mu, boş mu?

        if (!HasKitchenObject())
        {
            // CASE 1: Counter BOŞ - Objeyi spawn et ve Counter'a yerleştir.
            
            // Player'ın elinin boş olduğunu varsayıyoruz, çünkü aksi takdirde
            // Interact fonksiyonunun farklı bir mantık çalıştırması gerekirdi (objeyi bırakma).

            Transform objectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            KitchenObject ko = objectTransform.GetComponent<KitchenObject>();
            
            // Objenin yeni parent'ı Counter'dır.
            ko.transform.localPosition = Vector3.zero;
            SetKitchenObject(ko); // Counter objeyi artık kendisi tutuyor.
            OnGrabbedObject?.Invoke(this, EventArgs.Empty);
            Debug.Log("Domates tezgah üzerinde spawn edildi (E tuşu).");
        } 
        else 
        {
            // CASE 2: Counter DOLU - (Bu kısım ileride objeyi elden tezgaha bırakma mantığı olacak)
            Debug.Log("Tezgah dolu. Objeyi almak için T tuşuna basmalısın.");
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