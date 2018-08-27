using GNS3sharp;
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
        // Get the interface
        ushort iface = L1Mapping.DoorsOrderM1[CurrentNode][DoorNumber];
        // Extract the whole name of the node where this door is
        string nodeName = null;
        foreach (string nodeNameLoop in L1Mapping.NodeNamesM1) {
            if (nodeNameLoop.IndexOf(CurrentNode) != -1) {
                nodeName = nodeNameLoop;
                break;
            }
        }
        Node node = GNS3Handler.Instance.projectHandler.GetNodeByName(nodeName);

        // Set the sign
        // If the node is a router, it uses the network related to the next step interface for the sign
        OpenWRT router;
        if (node.GetType() == typeof(OpenWRT))
        {
            router = (OpenWRT)node;
            var text = router.GetIPByInterface($"eth{iface.ToString()}");
            if (text != null)
                DoorText.text = text[0];
            else
                DoorText.text = $"eth{iface.ToString()}";
        }
        // Otherwise, it just uses the interface as a string
        else
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
