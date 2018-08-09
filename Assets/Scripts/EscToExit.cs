using UnityEngine;

public class EscToExit : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // Quits the game
            Application.Quit();
        }
    }
}
