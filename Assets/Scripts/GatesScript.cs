using UnityEngine;

public class GatesScript : MonoBehaviour
{
    [SerializeField] private Vector3 openingDirection;
    [SerializeField] private float size = 0.65f;
    [SerializeField] private int keyNumber = 1;
    private float openingTime;
    private float openingTime1 = 3.0f; //in time
    private float openingTime2 = 10.0f;//out of time
    private bool isKeyCollected;
    private bool isKeyInTime;
    private bool isKeyInserted;
    private AudioSource openingSound1;  
    private AudioSource openingSound2;
    void Start()
    {
        isKeyInserted = false;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length > 0 ) { openingSound1 = audioSources[0]; }
        if (audioSources.Length > 1 ) { openingSound2 = audioSources[1]; }
      
        GameEventSystem.Subscribe(OnGameEvent);

        GameState.AddListener(OnGameStateChanged);


    }

    void Update()
    {
        if (isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openingTime * openingDirection);
            if (transform.localPosition.magnitude >= size)
            {
                if(openingSound1 != null && openingSound1.isPlaying) { openingSound1.Stop(); }
                if(openingSound2 != null && openingSound2.isPlaying) { openingSound2.Stop(); }
            }
        }

        if (openingSound1 != null && openingSound1.isPlaying || openingSound2 != null && openingSound2.isPlaying)
        {
            if (openingSound1 != null) openingSound1.volume = Time.timeScale == 0.0f ? 0.0f : GameState.effectsVolume;
            if (openingSound2 != null) openingSound2.volume = Time.timeScale == 0.0f ? 0.0f : GameState.effectsVolume;

            //openingSound1.volume = openingSound2.volume =
            //    Time.timeScale == 0.0f ? 0.0f : GameState.effectsVolume
            //;
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
                        toast = "ƒвер≥ в≥дчин€ютьс€...",
                    });

                    if (openingSound1 != null && openingSound2 != null && openingSound1.enabled && openingSound2.enabled)
                    {
                        (isKeyInTime ? openingSound1 : openingSound2).Play();
                    }
                    else if (openingSound1 != null && openingSound1.enabled)
                    {
                        openingSound1.Play();
                    }
                    else if (openingSound2 != null && openingSound2.enabled)
                    { 
                         openingSound2.Play();
                    }

                }

            }
            else
            {
                ToasterScript.Toast($"ƒвер≥ зачинен≥(ха-ха-ха).Ўукай  ключ {keyNumber} !");
            }

        }

    }

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.effectsVolume))
        {
            if (openingSound1 != null)
            { 
                openingSound1.volume = GameState.effectsVolume;
            }
            if (openingSound2 != null)
            {
                openingSound2.volume = GameState.effectsVolume;
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

        GameState.RemoveListener(OnGameStateChanged);

    }
}
