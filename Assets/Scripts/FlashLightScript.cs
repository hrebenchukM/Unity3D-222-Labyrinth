using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    private float charge;
    private float chargeLifeTime = 10.0f;

    /* Д.З. Реалізувати можливість зміни кута свічення 
     * ліхтарика за допомогою клавіатури / миші (на вибір)
     * у певних межах змін (підібрати практично)
     */
    private float minSpotAngle = 10.0f;
    private float maxSpotAngle = 90.0f;
    private float spotAngleStep = 5.0f;


    private bool isActive => !GameState.isDay && GameState.isFpv;
    void Start()
    {
        player = GameObject.Find("Player");
        if(player == null )
        {
            Debug.LogError("FlashLightScript: Player not found!");
        }
        _light = this.GetComponent<Light>();
        charge = 1.0f;
        ////////////////////////////////////////////////////////////////////////////
        _light.spotAngle = Mathf.Clamp(_light.spotAngle, minSpotAngle, maxSpotAngle);
        ////////////////////////////////////////////////////////////////////////////
    }

 
    void Update()
    {
        if (player == null) return;
        this.transform.position = player.transform.position;
        this.transform .forward = Camera.main.transform.forward;

        if (isActive)
        {
            charge = charge <0 ? 0.0f : charge -Time.deltaTime/chargeLifeTime;
            _light.intensity = charge;





            /////////////////////////////////////////////////////////////////////////////////////////////////
            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                _light.spotAngle = Mathf.Clamp(_light.spotAngle + spotAngleStep, minSpotAngle, maxSpotAngle);
            }
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                _light.spotAngle = Mathf.Clamp(_light.spotAngle - spotAngleStep, minSpotAngle, maxSpotAngle);
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////

        }
        else
        {
            _light.intensity = 0.0f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery")) 
        {
            BatteryScript battery = other.gameObject.GetComponent<BatteryScript>();

          
            //charge += 1.0f;
            charge = Mathf.Min(charge + battery.GetChargeAmount, 1.0f);
            //Debug.Log($"Added: {battery.GetChargeAmount}, Charge: {charge}");

            //Debug.Log("Charge:" + charge);
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = "Battery",
                toast = $"Ви знайшли батарейку. Заряд ліхтарика поповнено до {charge:F1}",
                sound = EffectsSounds.batteryCollected,

            });
            GameObject.Destroy(other.gameObject);
            //ToasterScript.Toast(
            //     $"Ви знайшли батарейку. Заряд ліхтарика поповнено до {charge:F1}",3.0f
            //);
        }
    }
}
