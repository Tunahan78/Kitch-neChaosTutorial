using UnityEngine;

[CreateAssetMenu(menuName = "KitchenChaos/Frying Recipe")]
public class FryingRecipeSO : ScriptableObject
{
    // Input: Tezgaha konulan hammadde (örn: Tomato)
    public KitchenObjectSO input; 

    // Output: Kesildikten sonraki ürün (örn: Sliced Tomato)
    public KitchenObjectSO output; 

    // Kaç etkileşimde (E tuşu basımında) dönüşüm olacak?
    public int fryProgressMax; 
}

