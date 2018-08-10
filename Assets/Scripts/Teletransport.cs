using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletransport : MonoBehaviour {

    [SerializeField]
    private float NewX;
    [SerializeField]
    private float NewY;
    [SerializeField]
    private Transform Character;

    private void OnTriggerStay2D() {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            Character.position = new Vector3(NewX, NewY);
        }
    }
}
