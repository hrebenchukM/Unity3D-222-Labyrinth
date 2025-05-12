using UnityEngine;
using UnityEngine.UI;

public class Key1Script : MonoBehaviour
{
    [SerializeField] private int keyNumber = 1;
    [SerializeField] private float timeout = 10.0f; // in sec
    private GameObject content;
    private Image timeoutImage;
    private float timeLeft;


    private bool timerActive = false;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        timeoutImage = transform.Find("Indicator/Canvas/Foreground").GetComponent<Image>();
        timeoutImage.fillAmount = 1.0f;
        timeLeft = timeout;


    }

    void Update()
    {
        if (!timerActive && GameState.LastOpenedGateNumber == keyNumber - 1)
        {
            timerActive = true;
        }

        if (timerActive && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeoutImage.fillAmount = Mathf.Clamp01(timeLeft/timeout);
            timeoutImage.color = new Color(
                      Mathf.Clamp01(2.0f * (1.0f - timeoutImage.fillAmount)),
                      Mathf.Clamp01(2.0f * timeoutImage.fillAmount),
                      0f,
                      timeoutImage.color.a
                );
            if(timeLeft <= 0 )
            {
                GameState.SetProperty($"IsKey{keyNumber}InTime", false);
            }
        }
        content.transform.Rotate( 0f , Time.deltaTime * 30f ,0f );
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if(other.name == "Player")
        {
            GameState.SetProperty($"IsKey{keyNumber}Collected",true);
            //GameState.IsKey1Collected = true;
            Destroy(this.gameObject);
        }
    }
}
