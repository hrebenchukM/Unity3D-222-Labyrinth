using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;
    public static int batteryCount = 0;

    private static PlayerScript prevInstance = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();


        if (prevInstance != null)
        {
            //GameObject.Destroy(this.gameObject);

            this.rb.linearVelocity = prevInstance.rb.linearVelocity;
            this.rb.angularVelocity = prevInstance.rb.angularVelocity;
            GameObject.Destroy(prevInstance.gameObject);
            prevInstance = this;
        }
        else
        {
            prevInstance = this;
        }


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

        rb.AddForce(Time.timeScale *
            (moveValue.x * cameraRight + moveValue.y * cameraForward) 
            * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery")) 
        {
            BatteryScript battery = other.gameObject.GetComponent<BatteryScript>();

            float amount = battery.GetChargeAmount;

            //charge += 1.0f;
            //charge = Mathf.Min(charge + battery.GetChargeAmount, 1.0f);
            //Debug.Log($"Added: {battery.GetChargeAmount}, Charge: {charge}");

            //Debug.Log("Charge:" + charge);

            if (GameState.bag.ContainsKey("BatteryCollected"))
            {
                GameState.bag["BatteryCollected"] += 1;
            }
            else
            {
                GameState.bag["BatteryCollected"] = 1;
            }
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = "Battery",
                payLoad = amount,
                toast = $"Ви знайшли батарейку. Заряд ліхтарика поповнено на {amount:F1}",
                sound = EffectsSounds.batteryCollected,

            });
            GameObject.Destroy(other.gameObject);
            //ToasterScript.Toast(
            //     $"Ви знайшли батарейку. Заряд ліхтарика поповнено до {charge:F1}",3.0f
            //);
        }
    }

}
/* Скрипт управління персонажем.
 * Базується на фізиці прикладання сили до 
 * сферичного тіла, що може котитись
 */