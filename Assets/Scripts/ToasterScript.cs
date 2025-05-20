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

    private float deltaTime = 0f;
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

        Debug.Log($"FPS:{Application.targetFrameRate},vSync:{QualitySettings.vSyncCount}, SFR:{Screen.currentResolution.refreshRateRatio}");
    }
    void Update()
    {
        if (deltaTime == 0f && Time.deltaTime != 0f)
        {
            deltaTime = Time.deltaTime;
        }
        if (timeount > 0f)
        {
            //if (timeount > (showTime - 1.0f)) 
            //{
            //    canvasGroup.alpha = Mathf.Clamp01((showTime - timeount) / 0.5f);
            //}
            //else if (timeount > 0.5f)
            //{
            //    canvasGroup.alpha = 1.0f;
            //}
            //else
            //{
            //    canvasGroup.alpha = Mathf.Clamp01(timeount / 0.5f);
            //}
            canvasGroup.alpha = Mathf.Clamp01(timeount * 2.0f);
            //timeount -= Time.deltaTime;
            float dt = Time.timeScale > 0.0f ? Time.deltaTime
                : this.deltaTime > 0f ? this.deltaTime
                : QualitySettings.vSyncCount > 0 ? QualitySettings.vSyncCount / (float)Screen.currentResolution.refreshRateRatio.value
                : Application.targetFrameRate > 0 ? 1.0f / Application.targetFrameRate
                : 0.016f;
            Debug.Log(dt);
            timeount -= dt;
            //timeount -= Time.timeScale > 0.0f ? Time.deltaTime : deltaTime;
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
