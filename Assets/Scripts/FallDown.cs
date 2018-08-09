using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour {

    [SerializeField]
    private float NewX;
    [SerializeField]
    private float NewY;
    [SerializeField]
    private Transform Character;
	
    private void OnTriggerEnter2D()  {
        Character.position = new Vector3(NewX,NewY);
    }
}
