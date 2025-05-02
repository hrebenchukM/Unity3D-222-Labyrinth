using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Rigidbody rb;
    private InputAction moveAction; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //rb.AddForce(moveValue.x, 0, moveValue.y); -- прив'язка до світових осей
        // (напрямів) -- незалежно від повороту камери рух іде вздовж постійних напрямів

        //орієнтація за камерою:
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;//прибираємо вертикальну компоненту (проектуємо на горизонтальну площину)
       
        if(cameraForward == Vector3.zero) //на випадок якщо камера ідеально згори тоді замінюємо forward -> up 
        {
            cameraForward=Camera.main.transform.up;
        }
        else
        {
            cameraForward.Normalize();
        }
        cameraForward.Normalize();//призводимо вектор до одиничного розміру (видовжуємо)
        
        Vector3 cameraRight = Camera.main.transform.right;//коригування не потребує ,оскільки має бути завжди горизонтальним 
        rb.AddForce(moveValue.x * cameraRight + moveValue.y * cameraForward);
    }
}
