// DeliveryRecipiUI.cs
using UnityEngine;

public class DeliveryRecipiUI : MonoBehaviour
{
    // === EDITOR REFERANSLARI ===
    [SerializeField] private Transform container;      // Sizin "Counter" objeniz
    [SerializeField] private Transform recipeTemplate; // Sizin "RecipeTemplate" objeniz

    private void Awake()
    {
        // Başlangıçta şablonu gizle, çünkü o sadece bir kopya kaynağı.
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        // 1. EVENT'LERE ABONE OL
        RecipeManager.Instance.OnRecipeSpawned += RecipeManager_OnRecipeSpawned;
        RecipeManager.Instance.OnRecipeCompleted += RecipeManager_OnRecipeCompleted;

        UpdateVisual(); // Başlangıçta UI'ı doğru duruma getir.
    }

    // Yeni tarif geldiğinde tetiklenir.
    private void RecipeManager_OnRecipeSpawned(object sender, RecipeManager.OnRecipeEventArgs e)
    {
        UpdateVisual();
    }

    // Tarif tamamlandığında tetiklenir.
    private void RecipeManager_OnRecipeCompleted(object sender, RecipeManager.OnRecipeEventArgs e)
    {
        UpdateVisual();
    }

    // UI'ı güncellemek için tek bir merkezi metot.
    private void UpdateVisual()
    {
        // 1. ÖNCE ESKİ KARTLARI TEMİZLE
        foreach (Transform child in container)
        {
            // Sadece şablonu (template) silme, diğerlerini sil.
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        // 2. SONRA GÜNCEL LİSTEYE GÖRE YENİ KARTLARI OLUŞTUR
        foreach (RecipeSO recipeSO in RecipeManager.Instance.GetWaitingRecipeSOList())
        {
            // Şablondan bir kopya oluştur.
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            
            // Kopyanın script'ine tarifi ver.
            recipeTransform.GetComponent<RecipeSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}