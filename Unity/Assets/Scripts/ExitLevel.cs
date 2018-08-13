using UnityEngine;

public class ExitLevel : MonoBehaviour {

    private void OnTriggerStay2D()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Application.Quit();
        }
    }
}
