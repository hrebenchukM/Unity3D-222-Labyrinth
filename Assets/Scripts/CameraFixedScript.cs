using UnityEngine;

public class CameraFixedScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraScript.isFixed = true;
            CameraScript.isFixed = this.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraScript.isFixed = false;
        }
    }
}
