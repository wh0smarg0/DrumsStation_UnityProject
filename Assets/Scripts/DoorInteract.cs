using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [Header("Кут відкриття (X, Y, Z)")]
    // За замовчуванням обертаємо на 90 градусів по осі Y
    public Vector3 openAngle = new Vector3(0, 90, 0); 

    void Start()
    {
        // Використовуємо localRotation, щоб шафа могла стояти під будь-яким кутом у кімнаті
        closedRotation = transform.localRotation; 
        openRotation = closedRotation * Quaternion.Euler(openAngle); 
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        transform.localRotation = isOpen ? openRotation : closedRotation;
    }
}