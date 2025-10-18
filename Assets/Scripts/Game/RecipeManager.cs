using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RecipeManager : MonoBehaviour
{
   public static RecipeManager Instance { get; private set; }

    public class OnRecipeEventArgs : EventArgs
    {
        public RecipeSO recipeSO;
    }


    //
    public event EventHandler<OnRecipeEventArgs> OnRecipeSpawned; // Görev Tanımlandı
    //
    public event EventHandler<OnRecipeEventArgs> OnRecipeCompleted; // Görev başarı ile tamamlandı
    //
    public event EventHandler OnRecipeFailed; // Görev başarısız


    private List<RecipeSO> waitRecipeSOList; // Ekranda bekleyen aktif tarifler

    [SerializeField] private List<RecipeSO> allRecipeSOList; // Oyundaki Tüm Listeler

    private float spawnTimer;
    [SerializeField] private float spawnwaitTime; // Kaç saneiyede bir görev spawn edilicek
    [SerializeField] private float waitingRecipeMax; // Ekranda maks görev sayısı

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        waitRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        if (waitRecipeSOList.Count < waitingRecipeMax) // aktif tarifler maksa ulaşmadıysa burdan devam et
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnwaitTime) // Görev spawnla
            {
                spawnTimer = 0;
                RecipeSO newWaitingRecipe = allRecipeSOList[UnityEngine.Random.Range(0, allRecipeSOList.Count)]; // Random bir tn tarif seç
                // Yeni bekleyenler tarafına ata
                waitRecipeSOList.Add(newWaitingRecipe);
                //UI ya haber ver yani Tarif atandı
                Debug.Log("Tarif atandı: " + newWaitingRecipe.kitchenObjectSOList );
                OnRecipeSpawned?.Invoke(this, new OnRecipeEventArgs{ recipeSO = newWaitingRecipe });
            }
        }
    }

    public bool TryComplateRecipe(PlateKitchenObject plateKitchenObject)
    {
        List<KitchenObjectSO> plateIngredients = plateKitchenObject.GetKitchenObjectSOList(); // Tabağımdaki objeleri bu listeye at
        // Bekleyen tarifler listesinde geriye doğru döngü yapıyoruz.
        // Bu, listeden eleman silerken hata almamızı engeller.

        for (int i = waitRecipeSOList.Count - 1; i >= 0; i--)
        {
            RecipeSO waitingRecipe = waitRecipeSOList[i];

            // 1. KURAL: Malzeme sayıları aynı mı?
            if (waitingRecipe.kitchenObjectSOList.Count == plateIngredients.Count)
            {
                // 2. KURAL: Tüm malzemeler eşleşiyor mu?
                bool allIngredientsMatch = true;
                foreach (KitchenObjectSO recipeIngredient in waitingRecipe.kitchenObjectSOList)
                {
                    // Eğer tarifteki bir malzeme tabakta yoksa...
                    if (!plateIngredients.Contains(recipeIngredient))
                    {
                        allIngredientsMatch = false;
                        break; // Eşleşme bozuldu, bu tarifi kontrol etmeyi bırak.

                    }
                }

                // Eğer tüm malzemeler eşleşiyorsa...
                if (allIngredientsMatch)
                {
                    // Sipariş tamamlandı!
                    waitRecipeSOList.RemoveAt(i); // Tamamlanan tarifi listeden sil.

                    OnRecipeCompleted?.Invoke(this, new OnRecipeEventArgs { recipeSO = waitingRecipe }); // Başarı olayını yayınla.
                    return true; // DeliveryCounter'a "Başarılı" bilgisini döndür.
                }
            }
        }

        // Döngü bitti ve hiçbir tarif eşleşmedi.
        OnRecipeFailed?.Invoke(this, EventArgs.Empty); // Başarısızlık olayını yayınla.
        return false; // DeliveryCounter'a "Başarısız" bilgisini döndür.
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        // Dışarıdan isteyen herkese, şu an bekleyen tariflerin listesini döndür.
        return waitRecipeSOList;
    }

    
}
