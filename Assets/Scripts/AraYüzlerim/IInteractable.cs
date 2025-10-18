using UnityEngine;

public interface IInteractable
{
    // Bütün tezgahlar bu metodu uygulamak zorundadır.
    // Interact metodu, PlayerInteraction objesini parametre olarak alır.
    void Interact(PlayerInteraction playerInteraction);

    void SetSelected(bool isSelected);
}
