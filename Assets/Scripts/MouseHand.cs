using UnityEngine;

public class MouseHand : MonoBehaviour
{
    public float reachDistance = 3f; // Як далеко дістає ваша "рука"
    public Transform holdPosition;   // Сюди перетягніть створений об'єкт HoldPosition

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Клік лівою кнопкою миші
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryInteract(); // Якщо руки пусті - пробуємо щось взяти або відкрити
            }
            else
            {
                DropObject();  // Якщо в руках щось є - кидаємо
            }
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            // ЦЯ лінія покаже в Консолі, у що саме вдарився наш лазер:
            Debug.Log("Промінь влучив у об'єкт: " + hit.collider.gameObject.name);

            DoorInteract door = hit.collider.GetComponent<DoorInteract>();
            if (door != null)
            {
                door.ToggleDoor();
                return;
            }
            
            DrawerInteract drawer = hit.collider.GetComponent<DrawerInteract>();
            if (drawer != null)
            {
                drawer.ToggleDrawer();
                return;
            }

            Rigidbody targetRb = hit.collider.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                // ... (тут залишається ваш старий код для взяття предметів)
                heldObject = hit.collider.gameObject;
                heldRb = targetRb;
                heldRb.useGravity = false;
                heldRb.isKinematic = true; 
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.parent = holdPosition;
            }
        }
        else 
        {
            Debug.Log("Промінь нікуди не дістав! (Занадто далеко або немає колайдера)");
        }
    }

    void DropObject()
    {
        // Вмикаємо гравітацію і відпускаємо предмет
        heldObject.transform.parent = null;
        heldRb.useGravity = true;
        heldRb.isKinematic = false;
        
        heldObject = null;
        heldRb = null;
    }
}