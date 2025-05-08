using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    private static ToasterScript instance;
    private GameObject content;
    private TMPro.TextMeshProUGUI text;
    private float showTime = 3.0f;
    private float timeount;
    void Start()
    {
        instance = this;
        Transform t = this.transform.Find("Content");
        content= t.gameObject;
        text = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content.SetActive(false);
    }
    void Update()
    {
        if (timeount > 0) 
        {
            Debug.Log(timeount);
            timeount -= Time.deltaTime;
            if (timeount <= 0f)
            { 
                content.SetActive(false);
            }

        }

    }
    public static void Toast (string message,float time = 0.0f)
    {
        instance.content.SetActive(true);
        instance.text.text = message;
        instance.timeount = time == 0.0f ? instance.showTime : time;
    }
}
