using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask countersLayer;
    [SerializeField] private GameInput gameInput;

    private IInteractable lastselectedCounter;
    private IInteractable selectedCounter;
    private Transform selectedCounterTransform;


    private void Awake()
    {
        // ÖNEMLİ: Etkileşim event'ine abone ol.
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnTakeAction += GameInput_OnTakeAction;
        gameInput.OnCuttingAction += GameInput_OnCuttingAction;
    }

    private void GameInput_OnCuttingAction()
    {
        if (selectedCounter != null)
        {
            if (selectedCounterTransform.TryGetComponent(out ICutting cuttingCounter))
            {
                cuttingCounter.Cutting();
                Debug.Log("CuttingCounter F tuşu ile kesme komutunu aldı.");
            }
            else
                Debug.Log("Seçili tezgah kesilemez.");
        }
    }

    private void GameInput_OnTakeAction()
    {
        // Sadece geçerli bir tezgah seçiliyse ve o tezgâhın üstü doluysa alma denemesi yap.
        if (selectedCounter != null)
        {
            // Kontrol 1: Oyuncunun eli boş mu?
            if (GetComponent<IKitchenObjectParent>().HasKitchenObject()) return;

            // Kontrol 2: Tezgahın üstü dolu mu? (ClearCounter da IKitchenObjectParent olduğu için)
            if (selectedCounterTransform.GetComponent<IKitchenObjectParent>().HasKitchenObject())
            {
                // Objeyi al ve taşı.
                TransferKitchenObject();
            }
        }
    
    }

    // Aktarım mantığını ayrı bir metoda taşıyarak SRP'yi koruyalım.
    private void TransferKitchenObject()
    {

        if (selectedCounterTransform == null) return;
        // Her iki taraf da IKitchenObjectParent olduğu için aktarım temizdir.
        IKitchenObjectParent playerHolder = GetComponent<IKitchenObjectParent>();
        IKitchenObjectParent counterHolder = selectedCounterTransform.GetComponent<IKitchenObjectParent>();

        KitchenObject kitchenObjectToTransfer = counterHolder.GetKitchenObject();

        // 1. Objenin tezgahtan çıktığını bildir.
        counterHolder.SetKitchenObject(null);

        // 2. Objenin Player'a geçtiğini bildir.
        playerHolder.SetKitchenObject(kitchenObjectToTransfer);

        // 3. Objenin transform'unu Player'ın tutma noktasına ayarla (Görünürlük Sorununu Çözer)
        kitchenObjectToTransfer.transform.parent = playerHolder.GetKitchenObjectFollowTransform();
        kitchenObjectToTransfer.transform.localPosition = Vector3.zero;

        Debug.Log("Domates T tuşu ile alındı ve aktarıldı!");
    }


    private void OnDestroy()
    {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction()
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }


    // Update is called once per frame
    void Update()
    {
        DetectInteractableCounter();

    }

    private void DetectInteractableCounter()
    {
        Vector3 playerForward = transform.forward;

        // Karakterin pozisyonundan playerForward yönüne doğru, belirlenen mesafeye kadar ışın atarak önümde bir collider olup olmadığını kontrol ediyorum.
        if (Physics.Raycast(transform.position, playerForward, out RaycastHit hit, interactionDistance, countersLayer))
        {
            // Çarptığım şey (örneğin bir kutu), benim belirlediğim IInteractable sözleşmesine sahip mi? Bu soruyu soruyorum.
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                selectedCounter = interactable;
                selectedCounterTransform = hit.transform;
                // 
            }
            else
            {
                // çarpan obje benim IInteractable ıma kayıtlı değil 
                selectedCounter = null;
                selectedCounterTransform = null;
            }
        }
        else
        {
            // birşeye çarpmadı demektir 
            selectedCounter = null;
            selectedCounterTransform = null;
        }

        if (selectedCounter != lastselectedCounter)
        {
            if (lastselectedCounter != null)
            {
                lastselectedCounter.SetSelected(false);
            }
            if (selectedCounter != null)
            {
                selectedCounter.SetSelected(true);
            }
            lastselectedCounter = selectedCounter;
        }
    }

}
