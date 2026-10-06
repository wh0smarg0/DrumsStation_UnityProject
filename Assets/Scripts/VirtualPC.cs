using UnityEngine;
using UnityEngine.UI;

public class VirtualPC : MonoBehaviour
{
    [Header("Екран та Інтерфейс")]
    public GameObject monitorScreen; // Ваш Canvas
    public Text terminalText;        // Текстове поле для набору
    public RectTransform cursorUI;   // Іконка курсора на екрані

    [Header("Фізичні об'єкти")]
    public Transform physicalMouse;  // 3D-модель мишки на столі
    public float mouseSensitivity = 1500f;
    
    private Vector3 lastMousePos;
    private bool isScreenOn = false;

    void Start()
    {
        // Вимикаємо екран при старті сцени
        monitorScreen.SetActive(false);
        if (physicalMouse != null) lastMousePos = physicalMouse.position;
    }

    void Update()
    {
        // 1. Увімкнення/Вимкнення екрана клавішею Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isScreenOn = !isScreenOn;
            monitorScreen.SetActive(isScreenOn);
        }

        if (!isScreenOn) return;

        // 2. Набір тексту з фізичної клавіатури
        foreach (char c in Input.inputString)
        {
            if (c == '\b' && terminalText.text.Length > 0) // Видалення (Backspace)
            {
                terminalText.text = terminalText.text.Substring(0, terminalText.text.Length - 1);
            }
            else if (c == '\n' || c == '\r') // Перехід на новий рядок (Enter)
            {
                terminalText.text += "\n";
            }
            else
            {
                terminalText.text += c; // Додавання символу
            }
        }

        // 3. Рух курсора від фізичної 3D-мишки
        if (physicalMouse != null && cursorUI != null)
        {
            Vector3 delta = physicalMouse.position - lastMousePos;
            // Рух по столу (вісі X та Z) переводимо в екранні координати (X та Y)
            cursorUI.anchoredPosition += new Vector2(delta.x, delta.z) * mouseSensitivity;
            lastMousePos = physicalMouse.position;
        }
    }
}