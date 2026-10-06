using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    [Header("Напрямок висування")]
    // Змінюйте ці значення в Inspector, якщо шухляда їде не в той бік
    public Vector3 pullOffset = new Vector3(0, 0, 0.4f); 

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + pullOffset;
    }

    public void ToggleDrawer()
    {
        isOpen = !isOpen;
        transform.localPosition = isOpen ? openPosition : closedPosition;
    }
}