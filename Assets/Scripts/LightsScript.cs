using System.Linq;
using UnityEngine;

public class LightsScript : MonoBehaviour
{
    private Light[] daylights;
    private Light[] nightlights;
    private bool isDay;

    void Start()
    {
        daylights = GameObject
            .FindGameObjectsWithTag("Day")
            .Select(x => x.GetComponent<Light>())
            .ToArray();
        nightlights = GameObject
           .FindGameObjectsWithTag("Night")
           .Select(x => x.GetComponent<Light>())
           .ToArray();
        isDay = true;
        SetLights();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            isDay = !isDay;
            SetLights();
        }
    }
    private void SetLights()
    {
        if (isDay)
        {
            foreach(Light light in daylights)
            {
                light.intensity = 1.0f;
            }
            foreach (Light light in nightlights)
            {
                light.intensity = 0.0f;
            }
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
        }
        else
        {
            foreach (Light light in daylights)
            {
                light.intensity = 0.0f;
            }
            foreach (Light light in nightlights)
            {
                light.intensity = 1.0f;
            }
            RenderSettings.ambientIntensity= 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
    }
}
