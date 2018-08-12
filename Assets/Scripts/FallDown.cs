using UnityEngine;

public class FallDown : MonoBehaviour {

    [SerializeField]
    private string CurrentNode;
    [SerializeField]
    private Transform Character;
	
    private void OnTriggerEnter2D()  {
        Character.position = new Vector3(
            L1Mapping.NodeRespawnM1[CurrentNode][0], L1Mapping.NodeRespawnM1[CurrentNode][1]
        );
    }
}
