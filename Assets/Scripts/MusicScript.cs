using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource music;


    private static MusicScript prevInstance = null;
    void Awake()
    {
        if (prevInstance != null && prevInstance != this)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        else
        {
            prevInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {


        music = GetComponent<AudioSource>();

        GameState.AddListener(OnGameStateChanged);
    }


    void Update()
    {
        
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.musicVolume))
        {
            music.volume = GameState.musicVolume;
        }
       
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}
