using UnityEngine;

public class Teletransport : MonoBehaviour {

    public float NewX {
        get;
        set;
    }

    public float NewY {
        get;
        set;
    }

    [SerializeField]
    private Transform Character;

    private void OnTriggerStay2D() {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            Character.position = new Vector3(NewX, NewY);
        }
    }
}
