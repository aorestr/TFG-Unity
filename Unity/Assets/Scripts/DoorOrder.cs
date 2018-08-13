using UnityEngine;
using UnityEngine.UI;

public class DoorOrder : MonoBehaviour {

    [SerializeField]
    private string CurrentNode;

    [SerializeField]
    private ushort DoorNumber;

    [SerializeField]
    private Text DoorText;

    void Start()
    {
        ushort iface = L1Mapping.DoorsOrderM1[CurrentNode][DoorNumber];
        // Set the sign
        DoorText.text = $"eth{iface.ToString()}";
        var NewLocation = this.GetComponent<Teletransport>();
        // Set the new respawn location
        NewLocation.NewX = L1Mapping.NodeRespawnM1[
            L1Mapping.NextNodeM1[CurrentNode][iface]
        ][0];
        NewLocation.NewY = L1Mapping.NodeRespawnM1[
            L1Mapping.NextNodeM1[CurrentNode][iface]
        ][1];
    }

}
