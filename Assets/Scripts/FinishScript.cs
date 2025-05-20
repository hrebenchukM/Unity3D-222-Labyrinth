using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if(other.name == "Player")
       {
            GameObject.DontDestroyOnLoad(other.gameObject);
            SceneManager.LoadScene(1);
       }
    }
}
