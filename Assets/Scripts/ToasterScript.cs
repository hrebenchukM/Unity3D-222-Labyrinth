using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    private static ToasterScript instance;
    private GameObject content;
    private TMPro.TextMeshProUGUI text;
    private CanvasGroup canvasGroup;
    private float showTime = 3.0f; // час показу повідомлення
    private float timeount;        // залишок часу
    private readonly Queue<ToastMessage> messageQueue = new Queue<ToastMessage>();
    void Start()
    {
        instance = this;
        Transform t = this.transform.Find("Content");
        content= t.gameObject;
        canvasGroup = content.GetComponent<CanvasGroup>();
        text = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content.SetActive(false);
        GameState.AddListener(OnGameStateChanged);
        GameEventSystem.Subscribe(OnGameEvent);
    }
    void Update()
    {
        if (timeount > 0)
        {
            if (timeount > (showTime - 1.0f)) 
            {
                canvasGroup.alpha = Mathf.Clamp01((showTime - timeount) / 0.5f);
            }
            else if (timeount > 0.5f)
            {
                canvasGroup.alpha = 1.0f;
            }
            else
            {
                canvasGroup.alpha = Mathf.Clamp01(timeount / 0.5f);
            }


            timeount -= Time.deltaTime;
            if (timeount <= 0f)
            {
                content.SetActive(false);
            }

        }
        else if (messageQueue.Count > 0)
        {

            var message = messageQueue.Dequeue();
            content.SetActive(true);
            text.text = message.text;
            timeount = message.time;
        }


    }

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
            Toast(GameState.isDay
                ?"Настав день"
                :"Настала ніч");
        }
       
    }
    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.toast != null)
        {
          Toast(gameEvent.toast);
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
        GameEventSystem.UnSubscribe(OnGameEvent);
    }

    public static void Toast (string message,float time = 0.0f)
    {
        //instance.content.SetActive(true);
        //instance.text.text = message;
        //instance.timeount = time == 0.0f ? instance.showTime : time;
        instance.messageQueue.Enqueue(new ToastMessage
        { 
            text = message,
            time = time == 0.0f ? instance.showTime : time
        }
        );
    }
    private class ToastMessage
    {
        public string text { get; set; }
        public float time { get; set; }
    }

}
