using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private int keyNumber = 1;
    [SerializeField] private float timeout = 10.0f; // in sec
    [SerializeField] private string gatesDescription = "відповідні";
    private GameObject content;
    private Image timeoutImage;
    private float timeLeft;
    private bool isKeyInTime = true;


    private bool timerActive;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        timeoutImage = transform.Find("Indicator/Canvas/Foreground").GetComponent<Image>();
        timeoutImage.fillAmount = 1.0f;
        timeLeft = timeout;
        timerActive = keyNumber == 1;
        GameEventSystem.Subscribe(OnGameEvent);

    }

    void Update()
    {
      

        if (timerActive && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeoutImage.fillAmount = Mathf.Clamp01(timeLeft / timeout);
            timeoutImage.color = new Color(
                      Mathf.Clamp01(2.0f * (1.0f - timeoutImage.fillAmount)),
                      Mathf.Clamp01(2.0f * timeoutImage.fillAmount),
                      0f,
                      timeoutImage.color.a
                );
            if (timeLeft <= 0)
            {
                //GameState.IsKey1InTime = false;
                //GameState.SetProperty($"IsKey{keyNumber}InTime", false);
                isKeyInTime = false;
            }
        }
        content.transform.Rotate(0f, Time.deltaTime * 30f, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name == "Player")
        {
            GameState.bag.Add($"Key{keyNumber}Collected",1);
            //GameState.SetProperty($"IsKey{keyNumber}Collected", true);
            //GameState.IsKey1Collected = true;
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = $"IsKey{keyNumber}Collected",
                payLoad = isKeyInTime,
                toast = $"Ви знайшли ключ %{keyNumber}. Можете відкрити {gatesDescription} двері. ",
                sound = isKeyInTime
                ?EffectsSounds.keyCollectedInTime 
                :EffectsSounds.keyCollectedOutOfTime,
            });
            Destroy(this.gameObject);
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.type == $"Gate{keyNumber - 1}Opening")
        {
            timerActive = true;
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe(OnGameEvent);
    }
}
