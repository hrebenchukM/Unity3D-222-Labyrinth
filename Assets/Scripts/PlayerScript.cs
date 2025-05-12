using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // rb.AddForce(moveValue.x, 0, moveValue.y);  -- прив'язка до світових осей 
        // (напрямів) -- незалежно від повороту камери рух іде вздовж постійних напрямів

        // орієнтація за камерою:
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;   // прибираємо вертикальну компоненту (проєктуємо на 
        // горизонтальну площину)
        if (cameraForward == Vector3.zero)  // на випадок якщо камера ідеально згори
        {                                   // тоді замінюємо forward на up
            cameraForward = Camera.main.transform.up;
        }
        else
        {
            cameraForward.Normalize();      // призводимо вектор до одиничного розміру (видовжуємо)
        }

        Vector3 cameraRight = Camera.main.transform.right;   // корегування не потребує
        // оскільки завжди має бути горизонтальним

        rb.AddForce((moveValue.x * cameraRight + moveValue.y * cameraForward) * 10f);
    }
}
/* Скрипт управління персонажем.
 * Базується на фізиці прикладання сили до 
 * сферичного тіла, що може котитись
 */