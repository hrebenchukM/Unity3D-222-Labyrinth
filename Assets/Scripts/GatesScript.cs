using UnityEngine;

public class GatesScript : MonoBehaviour
{
    [SerializeField] private Vector3 openingDirection;
    [SerializeField] private float size = 0.65f;
    [SerializeField] private int keyNumber = 1;
    private float openingTime;
    private float openingTime1 = 0.5f; //in time
    private float openingTime2 = 4.0f;//out of time
    private bool isKeyCollected;
    private bool isKeyInTime;
    private bool isKeyInserted;
    void Start()
    {
        isKeyInserted = false;
        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {

        if (isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openingTime * openingDirection);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {

            //bool isKeyCollected = (bool)
            //       GameState.GetProperty($"IsKey{keyNumber}Collected");

            if (isKeyCollected)
            {
                if (!isKeyInserted)
                {
                    //ToasterScript.Toast("ƒвер≥ в≥дчин€ютьс€...");
                    //bool isInTime = (bool)
                    //     GameState.GetProperty($"IsKey{keyNumber}InTime");
                    openingTime = isKeyInTime ? openingTime1 : openingTime2;
                    isKeyInserted = true;
                    Debug.Log("Speed: " + size / openingTime);

                    //GameState.LastOpenedGateNumber = keyNumber;
                    GameEventSystem.EmitEvent(new GameEvent
                    {
                        type = $"Gate{keyNumber}Opening",
                        payLoad = openingTime,
                        toast = "ƒвер≥ в≥дчин€ютьс€..."
                    });

                }

            }
            else
            {
                ToasterScript.Toast($"ƒвер≥ зачинен≥(ха-ха-ха).Ўукай  ключ {keyNumber} !");
            }

        }

    }
    private void OnGameEvent(GameEvent gameEvent)
    {
        if(gameEvent.type == $"IsKey{keyNumber}Collected")
        {
            isKeyCollected= true;
            isKeyInTime = (bool)gameEvent.payLoad;
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe(OnGameEvent);
    }
}
