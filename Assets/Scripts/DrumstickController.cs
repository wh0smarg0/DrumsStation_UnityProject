using UnityEngine;

public class DrumstickController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Hit")]
    public float hitForce = 5f;
    public float bounceForce = 1f;

    private Rigidbody rb;

    // Початковий стан палички
    private Vector3 startPosition;
    private Quaternion startRotation;

    // Mouse drag
    private float zCoord;
    private Vector3 offset;
    private bool isDragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("На Drumstick немає Rigidbody!");
            return;
        }

        // Запам'ятовуємо початковий стан
        startPosition = transform.position;
        startRotation = transform.rotation;

        // На старті паличка не падає
        rb.useGravity = false;
    }

    void Update()
    {
        if (rb == null)
            return;

        // =========================
        // КЕРУВАННЯ КЛАВІАТУРОЮ
        // =========================

        if (!isDragging)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveX, 0f, moveZ);

            rb.MovePosition(
                rb.position + movement * moveSpeed * Time.deltaTime
            );
        }

        // =========================
        // УДАР ПРОБІЛОМ
        // =========================

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit();
        }

        // =========================
        // RESET КНОПКОЮ R
        // =========================

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStick();
        }
    }

    // =====================================================
    // УДАР
    // =====================================================

    private void Hit()
    {
        Debug.Log("Удар!");

        // Вмикаємо фізику
        rb.useGravity = true;

        // Скидаємо попередній рух
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Різко опускаємо паличку вниз
        rb.AddForce(Vector3.down * hitForce, ForceMode.Impulse);
    }

    // =====================================================
    // MOUSE DOWN
    // =====================================================

    private void OnMouseDown()
    {
        if (Camera.main == null)
            return;

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        offset = transform.position - GetMouseAsWorldPoint();

        isDragging = true;

        // Коли беремо паличку мишкою,
        // вона більше не повинна падати
        rb.useGravity = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // =====================================================
    // MOUSE DRAG
    // =====================================================

    private void OnMouseDrag()
    {
        if (!isDragging)
            return;

        Vector3 targetPosition =
            GetMouseAsWorldPoint() + offset;

        rb.MovePosition(targetPosition);
    }

    // =====================================================
    // MOUSE UP
    // =====================================================

    private void OnMouseUp()
    {
        isDragging = false;

        // Після відпускання дозволяємо падіння
        rb.useGravity = true;
    }

    // =====================================================
    // GET MOUSE POSITION
    // =====================================================

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // =====================================================
    // COLLISION WITH DRUM
    // =====================================================

    private void OnCollisionEnter(Collision collision)
    {
        // Перевіряємо TAG барабана
        if (collision.gameObject.CompareTag("Drum"))
        {
            Debug.Log("Бам! Удар по барабану!");

            // Невеликий відскок
            rb.AddForce(
                Vector3.up * bounceForce,
                ForceMode.Impulse
            );
        }
    }

    // =====================================================
    // FALL TRIGGER
    // =====================================================

    private void OnTriggerEnter(Collider other)
    {
        // Перевіряємо TAG тригера
        if (other.CompareTag("FallTrigger"))
        {
            Debug.Log("Паличка впала! Повертаємо на місце.");

            ResetStick();
        }
    }

    // =====================================================
    // RESET
    // =====================================================

    private void ResetStick()
    {
        Debug.Log("Reset палички");

        // Повністю зупиняємо Rigidbody
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Повертаємо позицію
        rb.position = startPosition;

        // Повертаємо обертання
        rb.rotation = startRotation;

        // Вимикаємо гравітацію
        rb.useGravity = false;

        // Скидаємо стан перетягування
        isDragging = false;
    }
}