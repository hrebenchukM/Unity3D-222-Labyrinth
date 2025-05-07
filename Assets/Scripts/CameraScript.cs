using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraAnchor;   // точка прив'язки камери 
    // за якою вона рухається, навколо якої обертається та до якої наближається
    private Vector3 offset;  // вектор взамного розміщення персонажа та камери

    private InputAction lookAction;   // Рухи маніпулятора "миша"
    private float rotAngleY, rotAngleY0;
    private float rotSensitivityY = 10f;
    private float rotAngleX, rotAngleX0;
    private float rotSensitivityX = 5f;
    private float maxOffset = 10.0f;  // максимальна віддаль камери від поля (cameraAnchor)
    private float fpvRange = 1.5f;    // межа переходу FPV режиму
    private float fpvOffset = 0.01f;  // відстань до cameraAnchor в FPV режимі
    // private float minAngleX = 40f;
    // private float maxAngleX = 90f;
    // private float minAngleFpvX = -10f;
    // private float maxAngleFpvX = 40f;

    public static bool isFixed = false;
    public static Transform fixedTransform = null;

    void Start()
    {
        offset = cameraAnchor.position - transform.position;
        lookAction = InputSystem.actions.FindAction("Look");
        rotAngleY = rotAngleY0 = transform.eulerAngles.y;
        rotAngleX = rotAngleX0 = transform.eulerAngles.x;
        GameState.isFpv= offset.magnitude < fpvRange;
    }

    void Update()
    {
        if (isFixed && fixedTransform != null)
        {
            this.transform.position = fixedTransform.position;
            this.transform.rotation = fixedTransform.rotation;
        }
        else
        {
            // Наближення / віддалення
            Vector2 zoom = Input.mouseScrollDelta;
            if (zoom.y > 0 && offset.magnitude > fpvRange)        // наближення - колесо крутилось "вперед"
            {
                offset *= 0.9f;
                if (offset.magnitude < fpvRange)                  // переходимо до FPV
                {
                    offset *= fpvOffset / offset.magnitude;
                    GameState.isFpv = true;
                }
            }
            else if (zoom.y < 0 && offset.magnitude < maxOffset)  // віддалення
            {
                if (offset.magnitude < fpvRange)                  // виходимо з FPV
                {
                    offset *= fpvRange / offset.magnitude;
                    GameState.isFpv = false;
                }
                offset *= 1.1f;
            }

            // Обертання камери
            Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();
            //     new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            rotAngleY += rotSensitivityY * lookValue.x;
            rotAngleX -= rotSensitivityX * lookValue.y;

            transform.eulerAngles = new Vector3(rotAngleX, rotAngleY, 0f);  // самого лише
            // обертання камери недостатньо, оскільки вона губить персонажа з 
            // поля зору через наявність зміщення offset -- його теж треба 
            // повертати разом з поворотом камери


            // "Стеження" - переміщення разом з персонажем
            transform.position = cameraAnchor.position - // offset : без корекції на поворот
                Quaternion.Euler(rotAngleX - rotAngleX0, rotAngleY - rotAngleY0, 0f) * offset;
        }
    }
}
/* Управління камерою.
 * Основа - положення персонажа, а також
 * рухи миші, які обертають камеру.
 */
/* Д.З. Підібрати граничні кути для повороту камери 
 * по вертикалі
 * - не бачить горизонт
 * - не випускає персонаж з поля зору
 * Впровадити обмеження на обертання згідно з визначеними
 * граничними кутами.
 */