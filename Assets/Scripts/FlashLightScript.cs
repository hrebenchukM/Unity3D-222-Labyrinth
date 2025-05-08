using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    private float charge;
    private float chargeLifeTime=10.0f;
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
            GameObject.Destroy(other.gameObject);
            charge += 1.0f;
            //Debug.Log("Charge:" + charge);
            ToasterScript.Toast(
                 $"Ви знайшли батарейку. Заряд ліхтарика поповнено до {charge:F1}",2.0f
            );
        }
    }
}
