using UnityEngine;

public class Gates1Script : MonoBehaviour
{
    [SerializeField] private Vector3 openingDirection;
    [SerializeField] private float size = 0.65f;
    [SerializeField] private int keyNumber = 1;
    private float openingTime;
    private float openingTime1 = 0.5f; //in time
    private float openingTime2 = 4.0f;//out of time
    
    private bool isKeyInserted;
    void Start()
    {
        isKeyInserted = false;
    }

    void Update()
    {
      
        if (isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openingTime * openingDirection);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player") 
        {

            bool isKeyCollected = (bool)
                   GameState.GetProperty($"IsKey{keyNumber}Collected");

            if (isKeyCollected)
            {
                if (!isKeyInserted)
                {
                    ToasterScript.Toast("���� �����������...");
                    bool isInTime = (bool)
                         GameState.GetProperty($"IsKey{keyNumber}InTime");
                    openingTime = isInTime ? openingTime1 : openingTime2;
                    isKeyInserted = true;
                    Debug.Log("Speed: " + size / openingTime);

                    GameState.LastOpenedGateNumber = keyNumber;

                }

            }
            else
            {
                ToasterScript.Toast($"���� �������(��-��-��).�����  ���� {keyNumber} !");
            }

        }

    }
}
