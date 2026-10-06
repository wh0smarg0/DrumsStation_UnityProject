using UnityEngine;

public class ShelfSnapZone : MonoBehaviour
{
    public Transform snapPoint;
    
    [Header("Крок для складання в стопку (X, Y, Z)")]
    // За замовчуванням піднімаємо кожну наступну книжку на 5 см вгору
    public Vector3 stackOffset = new Vector3(0, 0.05f, 0); 
    
    private int itemCount = 0;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRb = other.GetComponent<Rigidbody>();
        
        // Якщо предмет падає (useGravity == true)
        if (targetRb != null && targetRb.useGravity == true)
        {
            // Просто робимо об'єкт кінематичним (він сам зупиниться)
            targetRb.isKinematic = true;

            // Вираховуємо позицію: базова точка + (висота книжки * кількість предметів)
            other.transform.position = snapPoint.position + (stackOffset * itemCount);
            
            // Вирівнюємо предмет
            other.transform.rotation = snapPoint.rotation;

            // Прив'язуємо до полиці
            other.transform.SetParent(transform);
            
            // Збільшуємо лічильник стопки
            itemCount++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Якщо ви забрали предмет кліком мишки, звільняємо місце
        if (other.GetComponent<Rigidbody>() != null)
        {
            itemCount--;
            if (itemCount < 0) itemCount = 0; // Запобіжник від мінусових значень
        }
    }
}