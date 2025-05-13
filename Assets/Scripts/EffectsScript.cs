using System.Security.Cryptography;
using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyCollectSound;
    private AudioSource batteryCollectSound;
    private AudioSource keyCollectOutOfTimeSound;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        keyCollectSound = audioSources[0];
        batteryCollectSound = audioSources[1];
        keyCollectOutOfTimeSound = audioSources[2];
        GameEventSystem.Subscribe(OnGameEvent);
    }
    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.sound != null)
        {
            switch (gameEvent.sound) 
            {
                case EffectsSounds.batteryCollected: batteryCollectSound.Play(); break;
                case EffectsSounds.keyCollectedInTime: keyCollectSound.Play(); break;
                case EffectsSounds.keyCollectedOutOfTime: keyCollectOutOfTimeSound.Play(); break;
                default:Debug.LogError("Undefined sound:"+ gameEvent.sound); break;
            }

        }
    }
    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe(OnGameEvent);
    }
}
