using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{
    [SerializeField] private Material daySkyBox;
    [SerializeField] private Material nightSkyBox;

    void Start()
    {
   
        RenderSettings.skybox = GameState.isDay ? daySkyBox : nightSkyBox;
        GameState.AddListener(OnGameStateChanged);

    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
          RenderSettings.skybox=GameState.isDay ? daySkyBox:nightSkyBox;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }

}
