// RecipeSingleUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSingleUI : MonoBehaviour
{
    // === EDITOR REFERANSLARI ===
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer; // Sizin "Images" objeniz
    
    private RecipeSO recipeSO; // Bu kartın temsil ettiği tarif

    // Bu metot, ana UI yöneticisi tarafından çağrılacak.
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        this.recipeSO = recipeSO;

        // 1. Tarif adını güncelle.
        recipeNameText.text = recipeSO.recipeName;

        // 2. ÖNCE TÜM İKONLARI GİZLE: Bir önceki tariften kalanları temizle.
        foreach (Transform child in iconContainer)
        {
            child.gameObject.SetActive(false);
        }

        // 3. SONRA GEREKLİ İKONLARI GÖSTER
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            // Tarifteki malzemenin adıyla eşleşen ikonu bul.
            Transform iconTransform = iconContainer.Find(kitchenObjectSO.objectName); 
            // NOT: KitchenObjectSO'daki objectName ile ikonun GameObject adı aynı olmalı!
            
            if (iconTransform != null)
            {
                // İkonu aktif et.
                iconTransform.gameObject.SetActive(true);
            }
        }
    }

    // Ana yöneticinin bu kartın hangi tarife ait olduğunu bilmesi için.
    public RecipeSO GetRecipeSO()
    {
        return recipeSO;
    }
}
