using UnityEngine;

public class SnapZone : MonoBehaviour
{
    public Transform snapPoint; // Точка, куди предмет стане ідеально рівно

    void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRb = other.GetComponent<Rigidbody>();
        
        // Перевіряємо, чи це фізичний об'єкт і чи він ЗАРАЗ падає 
        // (щоб зона не виривала предмет прямо з ваших рук)
        if (targetRb != null && targetRb.useGravity == true)
        {
            // Зупиняємо падіння
            //targetRb.linearVelocity = Vector3.zero;
            targetRb.isKinematic = true;

            // Розміщуємо рівно по центру нашої зони
            other.transform.position = snapPoint.position;
            other.transform.rotation = snapPoint.rotation;

            // Робимо предмет дочірнім до зони фіксації
            // Тепер він буде рухатися разом із шухлядою або дверцятами!
            other.transform.SetParent(transform);
        }
    }
}