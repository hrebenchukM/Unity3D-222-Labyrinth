using UnityEngine;
using UnityEngine.UI;

public class Key1Script : MonoBehaviour
{
    private GameObject content;
    private Image timeoutImage;
   
    void Start()
    {
        content = transform.Find("Content").gameObject;
        timeoutImage = transform.Find("Indicator/Canvas/Foreground").GetComponent<Image>();
        timeoutImage.fillAmount = 1.0f;
    }

    void Update()
    {
        content.transform.Rotate(0f,Time.deltaTime*30f,0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if(other.name == "Player")
        {
            GameState.IsKey1Collected = true;
            Destroy(this.gameObject);
        }
    }
}
