using UnityEngine;

[CreateAssetMenu(menuName = "KitchenChaos/Cutting Recipe")]
public class CuttingRecipeSO : ScriptableObject
{
    // Input: Tezgaha konulan hammadde (örn: Tomato)
    public KitchenObjectSO input; 

    // Output: Kesildikten sonraki ürün (örn: Sliced Tomato)
    public KitchenObjectSO output; 

    // Kaç etkileşimde (E tuşu basımında) dönüşüm olacak?
    public int cuttingProgressMax; 
}

