using UnityEngine;

public class LookAtUI : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Sahne'de 'MainCamera' etiketiyle bir kamera bulunamadı.");
        }
    }

    
    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

            // Çoğu zaman bu tek satır da yeterlidir:
            // transform.LookAt(mainCamera.transform.position);
        }
    }
}