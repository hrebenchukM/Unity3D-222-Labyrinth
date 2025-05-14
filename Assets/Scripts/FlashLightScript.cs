using System.Security.Cryptography;
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
        GameEventSystem.Subscribe(OnGameEvent);
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
  

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.type == "Battery")
        {
            charge = Mathf.Min(charge + (float)gameEvent.payLoad, 1.0f);
            _light.intensity = charge;
            Debug.Log($"Added: {(float)gameEvent.payLoad}, Charge: {charge}");
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe(OnGameEvent);
    }
}
