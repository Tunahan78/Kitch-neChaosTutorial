using System;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;


public class StoveCounter : MonoBehaviour, IInteractable, IKitchenObjectParent

{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float processNormalizedEvent;
    }
    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private Transform orbitalPoint;
    [SerializeField] private FryingRecipeSO[] fryingRecipeArray;
    [SerializeField] private GameObject stoveOnVisual;
    [SerializeField] private ParticleSystem stoveParticle;
    private KitchenObject kitchenObject; // tezgahın şu an tuttuğu obje
    private State currentState; // tezhagın şuanki durumu 
    private float stateTimer; // pişirme süresindeki zamanlayıcı
    private FryingRecipeSO fryingRecipeSO; // geçerli pişirme tarifi

    public enum State
    {
        Idle, // tezgah boş
        Frying, // pişirme işlemi
        Freid, // pişmişten yanmaya doğru
        Burned // yanmış
    }

    private void Start()
    {
        SetSelected(false);
        currentState = State.Idle;

    }
    private void Update()
    {
        GetFrying();
        OnStoveVisual();
    }
    private void OnStoveVisual()
    {
        if (currentState == State.Burned)
        {
            stoveOnVisual.SetActive(false);
            stoveParticle.gameObject.SetActive(false);
        }
        else
        {
            stoveOnVisual.SetActive(currentState != State.Idle);
            stoveParticle.gameObject.SetActive(currentState != State.Idle);
        }
    }
    private void GetFrying()
    {
        if (HasKitchenObject() && fryingRecipeSO != null)
        {
            switch (currentState)
            {

                case State.Frying:
                    stateTimer += Time.deltaTime;
                    float processNormalized = stateTimer / fryingRecipeSO.fryProgressMax;
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { processNormalizedEvent = processNormalized });

                    if (stateTimer >= fryingRecipeSO.fryProgressMax)
                    {
                        // pişirme tamamlandı
                        kitchenObject.DestroySelf();
                        KitchenObjectSO outputObjectSO = fryingRecipeSO.output;
                        Transform outputObjectTransfom = Instantiate(outputObjectSO.prefab);
                        KitchenObject outputKitchenObject = outputObjectTransfom.GetComponent<KitchenObject>();
                        outputKitchenObject.SetKitchenObjectParent(this);
                        currentState = State.Freid;
                        stateTimer = 0f;
                        fryingRecipeSO = GetFryingRecipeSOWithInput(outputObjectSO);
                    }

                    break;
                case State.Freid:
                    stateTimer += Time.deltaTime;
                    processNormalized = stateTimer / fryingRecipeSO.fryProgressMax;
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { processNormalizedEvent = processNormalized });
                    if (stateTimer >= fryingRecipeSO.fryProgressMax)
                    {
                        // yanma tamamlandı
                        kitchenObject.DestroySelf();
                        KitchenObjectSO outputObjectSO = fryingRecipeSO.output;
                        Transform outputObjectTransfom = Instantiate(outputObjectSO.prefab);
                        KitchenObject outputKitchenObject = outputObjectTransfom.GetComponent<KitchenObject>();
                        outputKitchenObject.SetKitchenObjectParent(this);
                        currentState = State.Burned;
                        stateTimer = 0f;
                        fryingRecipeSO = null;
                        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { processNormalizedEvent = 0f });

                    }
                    break;
                case State.Burned:
                    break;

            }
        }
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObject)
    {
        foreach (FryingRecipeSO fryingRecipe in fryingRecipeArray)
        {
            if (fryingRecipe.input == inputKitchenObject)
            {
                return fryingRecipe;
            }

        }
        return null;
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObject)
    {
        return GetFryingRecipeSOWithInput(inputKitchenObject) != null;
    }
    // IInteractable implementation

    public void Interact(PlayerInteraction playerInteraction)
    {
        IKitchenObjectParent playerHolder = playerInteraction.GetComponent<IKitchenObjectParent>();
        if (!HasKitchenObject())
        {
            if (playerHolder.HasKitchenObject())
            {
                KitchenObjectSO inputObjectSO = playerHolder.GetKitchenObject().GetKitchenObjectSO();
                if (HasRecipeWithInput(inputObjectSO))
                {
                    playerHolder.GetKitchenObject().SetKitchenObjectParent(this);
                    playerHolder.SetKitchenObject(null);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(inputObjectSO);
                    currentState = State.Frying;
                    stateTimer = 0f;
                }
            }
        }
        else // Masa dolu değilse 
        {
            // CASE 2: Counter DOLU 
            KitchenObject heldObject = playerHolder.GetKitchenObject(); // Oyuncudaki obje
            KitchenObject counterObject = GetKitchenObject(); // masadaki obje

            if (playerHolder.HasKitchenObject() && heldObject is PlateKitchenObject playerPlate) // Oyuncuda tabak var masada malzeme var
            {
                if (playerPlate.TryAddIngredient(counterObject))
                {
                    SetKitchenObject(null);
                    fryingRecipeSO = null;
                    currentState = State.Idle;
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { processNormalizedEvent = 0f });
                    stateTimer = 0f;
                    return;
                }
                return;
            }

            else if (!playerHolder.HasKitchenObject()) // Masada obje var oyuncunun eli dolu 
            {
                GetKitchenObject().SetKitchenObjectParent(playerHolder);
                SetKitchenObject(null);
                fryingRecipeSO = null;
                currentState = State.Idle;
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { processNormalizedEvent = 0f });
                stateTimer = 0f;
                return;
            }
        }
    }
    
    public void SetSelected(bool isSelected)
    {
        selectedVisual.SetActive(isSelected);
    }
    // IKitchenObjectParent implementation
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return orbitalPoint;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject= kitchenObject;
    }

}

        