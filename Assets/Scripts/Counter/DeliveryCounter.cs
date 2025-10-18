using UnityEngine;

public class DeliveryCounter : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        SetSelected(false);
    }



    public void Interact(PlayerInteraction playerInteraction)
    {
        IKitchenObjectParent playerHolder = playerInteraction.GetComponent<IKitchenObjectParent>();
        if (!playerHolder.HasKitchenObject())
        {
            return;
        }
        KitchenObject holderObject = playerHolder.GetKitchenObject();
        if (holderObject is PlateKitchenObject plateObject)
        {
            // Oyuncunun Elindeki Tabak 
            // Sipariş Kontrol Et
            bool deliverySuccess = RecipeManager.Instance.TryComplateRecipe(plateObject); // burda kontrol yetkisini RecipeManagera verdik
            if (deliverySuccess)
            {
                //Tarif Doğru Burda ses çal denebilir ilerde 
                playerHolder.SetKitchenObject(null);
                Destroy(holderObject.gameObject);
                Debug.Log("Sipariş Başarıyla Teslim Edildi!");
            }
            else
            {
                //Tarif doğru değil ilerde yanlış ses çal fln eklenebilir
                playerHolder.SetKitchenObject(null);// Burda DestroySelf fonksiyonu olabilir ama ben o fonksiyonun doğru çalıştığını sanmıyorum
                Destroy(holderObject.gameObject);
                Debug.Log("Yanlış Tarif!");
            }
        }
        else
        {
            Debug.Log("Teslimat sadece tabakla yapılabilir!");
        }


    }
    public void SetSelected(bool isSelected)
    {
        selectedVisual.SetActive(isSelected);
    }
    
}
