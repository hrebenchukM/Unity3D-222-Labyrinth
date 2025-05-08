using UnityEngine;

public class Gates1Script : MonoBehaviour
{
    private float size = 0.8f;
    private float openingTime = 3.0f;
    private Vector3 openingDirection;
    private bool isKeyInserted;
    void Start()
    {
        isKeyInserted = false;
        openingDirection = Vector3.forward;
    }

    void Update()
    {
        if (isKeyInserted && transform.localPosition.magnitude <size)
        { 
           transform.Translate(size * Time.deltaTime / openingTime * openingDirection);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player") 
        { 
            if(GameState.IsKey1Collected)
            {
                ToasterScript.Toast("ƒвер≥ в≥дчин€ютьс€...");
                isKeyInserted=true;
            }
            else
            {
                ToasterScript.Toast("ƒвер≥ зачинен≥(ха-ха-ха).Ўукай син≥й ключ!");
            }

        }

    }
}
