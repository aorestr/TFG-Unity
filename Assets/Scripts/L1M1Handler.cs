using UnityEngine;
using GNS3sharp;
using System.Threading;

public class L1M1Handler : MonoBehaviour {

    private GNS3sharp.GNS3sharp GNS3instance = GNS3Handler.Instance.projectHandler;
    private ushort[] netsPrefix = GNS3Handler.RandomizeNets(8);

    // Use this for initialization
    void Start () {

        // Activate nodes
        foreach (string nodeName in L1NodesNames.M1) {
            GNS3instance.StartNode(
                GNS3instance.GetNodeByName(nodeName)
            );
        }

        // Assign every node with a variable
        VPC PC1 = (VPC)GNS3instance.GetNodeByName(L1NodesNames.M1[0]);
        VPC PC2 = (VPC)GNS3instance.GetNodeByName(L1NodesNames.M1[1]);
        OpenWRT R1 = (OpenWRT)GNS3instance.GetNodeByName(L1NodesNames.M1[2]);
        OpenWRT R2 = (OpenWRT)GNS3instance.GetNodeByName(L1NodesNames.M1[3]);
        OpenWRT R3 = (OpenWRT)GNS3instance.GetNodeByName(L1NodesNames.M1[4]);
        OpenWRT R4 = (OpenWRT)GNS3instance.GetNodeByName(L1NodesNames.M1[5]);
        OpenWRT R5 = (OpenWRT)GNS3instance.GetNodeByName(L1NodesNames.M1[6]);

        // We need to wait. Otherwise, nodes won't boot properly
        Thread.Sleep(120000);

        // Set up end-point nodes
        HelpL1M1Handler.SetUpPCs(new VPC[2] { PC1, PC2 }, netsPrefix);
        // Set-up routers
        HelpL1M1Handler.SetUpRouters(
            new OpenWRT[5] {R1, R2, R3, R4, R5}, netsPrefix
        );
    }

    private static class HelpL1M1Handler {

        public static void SetUpPCs(VPC[] PCs, ushort[] netsPrefix) {
            PCs[0].SetIP(
                IP: $"192.168.{netsPrefix[0]}.11", gateway: $"192.168.{netsPrefix[0]}.1"
            );
            PCs[1].SetIP(
                IP: $"192.168.{netsPrefix[netsPrefix.Length - 1]}.11", gateway: $"192.168.{netsPrefix[netsPrefix.Length - 1]}.2"
            );
        }

        public static void SetUpRouters(OpenWRT[] Routers, ushort[] netsPrefix) {
            // Net interfaces
            Routers[0].ActivateInterface(IP: $"192.168.{netsPrefix[0]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            Routers[0].ActivateInterface(IP: $"192.168.{netsPrefix[2]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            Routers[0].ActivateInterface(IP: $"192.168.{netsPrefix[1]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            Routers[0].DisableFirewall();
            //
            Routers[1].ActivateInterface(IP: $"192.168.{netsPrefix[1]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            Routers[1].ActivateInterface(IP: $"192.168.{netsPrefix[3]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            Routers[1].ActivateInterface(IP: $"192.168.{netsPrefix[4]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            Routers[1].DisableFirewall();
            //
            Routers[2].ActivateInterface(IP: $"192.168.{netsPrefix[5]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            Routers[2].ActivateInterface(IP: $"192.168.{netsPrefix[4]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            Routers[2].ActivateInterface(IP: $"192.168.{netsPrefix[2]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            Routers[2].DisableFirewall();
            //
            Routers[3].ActivateInterface(IP: $"192.168.{netsPrefix[3]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            Routers[3].ActivateInterface(IP: $"192.168.{netsPrefix[6]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            Routers[3].DisableFirewall();
            //
            Routers[4].ActivateInterface(IP: $"192.168.{netsPrefix[6]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            Routers[4].ActivateInterface(IP: $"192.168.{netsPrefix[7]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            Routers[4].ActivateInterface(IP: $"192.168.{netsPrefix[5]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            Routers[4].DisableFirewall();
            // Routes
            Routers[0].SetRoute(destination: $"192.168.{netsPrefix[7]}.0", gateway: $"192.168.{netsPrefix[2]}.3", netmask: "255.255.255.0");
            Routers[0].SetRoute(destination: $"192.168.{netsPrefix[5]}.0", gateway: $"192.168.{netsPrefix[2]}.3", netmask: "255.255.255.0");
            Routers[0].SetRoute(destination: $"192.168.{netsPrefix[3]}.0", gateway: $"192.168.{netsPrefix[1]}.1", netmask: "255.255.255.0");
            Routers[0].SetRoute(destination: $"192.168.{netsPrefix[4]}.0", gateway: $"192.168.{netsPrefix[2]}.3", netmask: "255.255.255.0");
            Routers[0].SetRoute(destination: $"192.168.{netsPrefix[6]}.0", gateway: $"192.168.{netsPrefix[1]}.1", netmask: "255.255.255.0");
            //
            Routers[1].SetRoute(destination: $"192.168.{netsPrefix[0]}.0", gateway: $"192.168.{netsPrefix[1]}.3", netmask: "255.255.255.0");
            Routers[1].SetRoute(destination: $"192.168.{netsPrefix[2]}.0", gateway: $"192.168.{netsPrefix[4]}.2", netmask: "255.255.255.0");
            Routers[1].SetRoute(destination: $"192.168.{netsPrefix[5]}.0", gateway: $"192.168.{netsPrefix[4]}.2", netmask: "255.255.255.0");
            Routers[1].SetRoute(destination: $"192.168.{netsPrefix[6]}.0", gateway: $"192.168.{netsPrefix[3]}.1", netmask: "255.255.255.0");
            Routers[1].SetRoute(destination: $"192.168.{netsPrefix[7]}.0", gateway: $"192.168.{netsPrefix[3]}.1", netmask: "255.255.255.0");
            //
            Routers[2].SetRoute(destination: $"192.168.{netsPrefix[7]}.0", gateway: $"192.168.{netsPrefix[5]}.3", netmask: "255.255.255.0");
            Routers[2].SetRoute(destination: $"192.168.{netsPrefix[0]}.0", gateway: $"192.168.{netsPrefix[2]}.2", netmask: "255.255.255.0");
            Routers[2].SetRoute(destination: $"192.168.{netsPrefix[1]}.0", gateway: $"192.168.{netsPrefix[2]}.2", netmask: "255.255.255.0");
            Routers[2].SetRoute(destination: $"192.168.{netsPrefix[3]}.0", gateway: $"192.168.{netsPrefix[4]}.3", netmask: "255.255.255.0");
            Routers[2].SetRoute(destination: $"192.168.{netsPrefix[6]}.0", gateway: $"192.168.{netsPrefix[5]}.3", netmask: "255.255.255.0");
            //
            Routers[4].SetRoute(destination: $"192.168.{netsPrefix[0]}.0", gateway: $"192.168.{netsPrefix[5]}.1", netmask: "255.255.255.0");
            Routers[4].SetRoute(destination: $"192.168.{netsPrefix[1]}.0", gateway: $"192.168.{netsPrefix[5]}.1", netmask: "255.255.255.0");
            Routers[4].SetRoute(destination: $"192.168.{netsPrefix[2]}.0", gateway: $"192.168.{netsPrefix[5]}.1", netmask: "255.255.255.0");
            Routers[4].SetRoute(destination: $"192.168.{netsPrefix[3]}.0", gateway: $"192.168.{netsPrefix[6]}.2", netmask: "255.255.255.0");
            Routers[4].SetRoute(destination: $"192.168.{netsPrefix[4]}.0", gateway: $"192.168.{netsPrefix[5]}.1", netmask: "255.255.255.0");
        }

    }

}
