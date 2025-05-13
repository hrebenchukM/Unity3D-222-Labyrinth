using UnityEngine;

public class CameraFixedScript : MonoBehaviour
{
    [SerializeField] private Transform[] positionsFixed;
    private int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraScript.isFixed = true;
            CameraScript.fixedTransform = this.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraScript.isFixed = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
                index = (index - 1 + positionsFixed.Length) % positionsFixed.Length;
                if (CameraScript.isFixed)
                {
                    CameraScript.fixedTransform = positionsFixed[index];
                    
                }

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
                index = (index + 1) % positionsFixed.Length;
                if (CameraScript.isFixed)
                {
                    CameraScript.fixedTransform = positionsFixed[index];
                 
                }
               
        }
    }
}