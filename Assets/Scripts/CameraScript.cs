using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraAnchor; //точка прив'язки камери
    //за якою вона рухається, навколо якої обертається та до якої наближається
    private Vector3 offset;//вектор взаємного розміщення персонажа та камери
    private InputAction lookAction;//рухи маніпулятора миша
    private float rotationAngleY;
    private float rotationAngleX;
    private float rotationSensitivityY = 10f;
    private float rotationSensitivityX = 5f;

    private float minVerticalAngle = 10f;
    private float maxVerticalAngle = 80f;
    void Start()
    {
        offset = cameraAnchor.position - transform.position;
        lookAction = InputSystem.actions.FindAction("Look");
        rotationAngleY = 0f;
        rotationAngleX = transform.eulerAngles.x;
    }

   
    void Update()
    {
        Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();
        rotationAngleY += rotationSensitivityY * lookValue.x;
        rotationAngleX += rotationSensitivityX * lookValue.y;

        if (rotationAngleX < minVerticalAngle)
        {
            rotationAngleX = minVerticalAngle;
        }
        else if (rotationAngleX > maxVerticalAngle)
        {
            rotationAngleX = maxVerticalAngle;
        }


        transform.eulerAngles = new Vector3(rotationAngleX, rotationAngleY, 0f);//самого лише обертання камери недостатньо,
                                                                     //оскільки вона губить персонажа з поля зору через наявність offset
                                                                     // --його теж треба повертати разом з поворотом камери
        transform.position = cameraAnchor.position - //offset - без коррекції на поворот
         Quaternion.Euler(0f,rotationAngleY,0f) * offset;
    }
}
