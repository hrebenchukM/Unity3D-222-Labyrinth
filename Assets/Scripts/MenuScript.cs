using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Image[] bagImages;
    private TMPro.TextMeshProUGUI _batteryCountText;

    private GameObject content;
    private Slider musicSlider;
    private Slider effectsSlider;
    private Slider singleEffectsSlider;
    private Toggle muteToggle;

    private float defaultMusicVolume;
    private float defaultEffectsVolume;
    private float defaultSingleEffectVolume;
    private bool defaultIsMuted;
    void Start()
    {
        defaultEffectsVolume = GameState.effectsVolume;
        defaultMusicVolume = GameState.musicVolume;
        defaultSingleEffectVolume = GameState.singleEffectsVolume;

        content = transform.Find("Content").gameObject;
        musicSlider = transform.Find("Content/Sounds/MusicSlider").GetComponent<Slider>();
        effectsSlider = transform.Find("Content/Sounds/EffectsSlider").GetComponent<Slider>();
        singleEffectsSlider = transform.Find("Content/Sounds/SingleEffectsSlider").GetComponent<Slider>();
        muteToggle = transform.Find("Content/Sounds/MuteToggle").GetComponent<Toggle>();
        _batteryCountText = transform.Find("Content/BatteryImage/Title").GetComponent<TMPro.TextMeshProUGUI>();
        defaultIsMuted = muteToggle.isOn;

        LoadPreferences();
        OnMuteChanged(muteToggle.isOn);

        Hide();
    }

    private void LoadPreferences()
    {
        if (PlayerPrefs.HasKey(nameof(muteToggle)))
        {
          muteToggle.isOn = PlayerPrefs.GetInt(nameof(muteToggle))==1;
        }
        if (PlayerPrefs.HasKey(nameof(GameState.musicVolume)))
        {
            musicSlider.value = GameState.musicVolume =
                PlayerPrefs.GetFloat(nameof(GameState.musicVolume));
        }
        else
        {
            musicSlider.value = GameState.musicVolume;
        }

        if (PlayerPrefs.HasKey(nameof(GameState.effectsVolume)))
        {
            effectsSlider.value = GameState.effectsVolume =
                PlayerPrefs.GetFloat(nameof(GameState.effectsVolume));
        }
        else
        {
            effectsSlider.value = GameState.effectsVolume;
        }
        if (PlayerPrefs.HasKey(nameof(GameState.singleEffectsVolume)))
        {
            singleEffectsSlider.value = GameState.singleEffectsVolume =
                PlayerPrefs.GetFloat(nameof(GameState.singleEffectsVolume));
        }
        else
        {
            singleEffectsSlider.value = GameState.singleEffectsVolume;
        }

    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (content.activeInHierarchy)
            {
                Hide();
            }
            else
            {
                Show();
            }

        }
    }
    private void Hide()
    {
        content.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void Show()
    {
        content.SetActive(true);
        Time.timeScale = 0.0f;
        int count = 0;
        if (GameState.bag.ContainsKey("BatteryCollected"))
            count = GameState.bag["BatteryCollected"];
        _batteryCountText.text = $"{count}";

        for (int i = 0; i < bagImages.Length; i++) 
        {
            if (GameState.bag.ContainsKey($"Key{i + 1}Collected"))
            {
                bagImages[i].enabled = true;
            }
            else 
            {
                bagImages[i].enabled = false;
            }
          
        }
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
    public void OnDefaultsClick()
    {
        muteToggle.isOn = defaultIsMuted;

        singleEffectsSlider.value = defaultSingleEffectVolume;
        effectsSlider.value =  defaultEffectsVolume;
        GameState.effectsVolume = muteToggle ? 0.0f : defaultEffectsVolume;
        musicSlider.value = defaultMusicVolume;
        GameState.musicVolume = muteToggle ? 0.0f : defaultMusicVolume;

        GameState.singleEffectsVolume = muteToggle ? 0.0f : defaultSingleEffectVolume;

    }
    public void OnContinueClick()
    {
        Hide();
    }
    public void OnEffectsVolumeChanged(float volume)
    {
        if(!muteToggle.isOn) GameState.effectsVolume = volume;
    }
    public void OnSingleEffectVolumeChanged(float volume)
    {
        if (!muteToggle.isOn) GameState.singleEffectsVolume = volume;
    }
    public void OnMusicVolumeChanged(float volume)
    {
        if (!muteToggle.isOn) GameState.musicVolume = volume;
    }
    public void OnMuteChanged(bool isMuted)
    {
        if (isMuted)
        {
            GameState.musicVolume = 0f;
            GameState.effectsVolume = 0f;
            GameState.singleEffectsVolume = 0f;
        }
        else
        {
            GameState.musicVolume = musicSlider.value;
            GameState.effectsVolume = effectsSlider.value;
            GameState.singleEffectsVolume = singleEffectsSlider.value;
        }

    }

    public void OnDestroy()
    {
        PlayerPrefs.SetFloat(nameof(GameState.musicVolume), musicSlider.value);
        PlayerPrefs.SetFloat(nameof(GameState.effectsVolume), effectsSlider.value);
        PlayerPrefs.SetFloat(nameof(GameState.singleEffectsVolume), singleEffectsSlider.value);
        PlayerPrefs.SetInt(nameof(muteToggle), muteToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
