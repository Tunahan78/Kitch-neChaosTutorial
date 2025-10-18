using UnityEngine;

public class TrashCounter : MonoBehaviour , IInteractable
{

    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        SetSelected(false);
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
       IKitchenObjectParent playerHolder = playerInteraction.GetComponent<IKitchenObjectParent>();
        // Temel Mantık: Counter (Tezgah) dolu mu, boş mu?

        if (playerHolder.HasKitchenObject())
        {
            // Oyuncunun elinde obje var, counter boş. Objeyi tezgaha koy.
            KitchenObject kitchenObjectToTransfer = playerHolder.GetKitchenObject();
            playerHolder.SetKitchenObject(null);
            Destroy(kitchenObjectToTransfer.gameObject);
        }
    }

    public void SetSelected(bool isSelected)
    {
        selectedVisual.SetActive(isSelected);
    }
}
