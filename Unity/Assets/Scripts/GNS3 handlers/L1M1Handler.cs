using UnityEngine;
using UnityEngine.UI;
using GNS3sharp;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

public class L1M1Handler : MonoBehaviour {

    private ushort[] NetsPrefix = L1Mapping.RandomizeNets(8);
    
    [SerializeField]
    private GameObject[] NetTables;

    [SerializeField]
    private Text DestinationIP;

    [SerializeField]
    private Text Counter;

    private uint time = 0;

    // Use this for initialization
    void Start () {

        // Activate nodes
        foreach (string nodeName in L1Mapping.NodeNamesM1) {
            GNS3Handler.Instance.projectHandler.StartNode(
                GNS3Handler.Instance.projectHandler.GetNodeByName(nodeName)
            );
        }

        // Assign every node with a variable
        VPC PC1 = (VPC)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[0]);
        VPC PC2 = (VPC)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[1]);
        OpenWRT R1 = (OpenWRT)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[2]);
        OpenWRT R2 = (OpenWRT)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[3]);
        OpenWRT R3 = (OpenWRT)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[4]);
        OpenWRT R4 = (OpenWRT)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[5]);
        OpenWRT R5 = (OpenWRT)GNS3Handler.Instance.projectHandler.GetNodeByName(L1Mapping.NodeNamesM1[6]);
        OpenWRT[] Routers = new OpenWRT[5] { R1, R2, R3, R4, R5 };

        // We need to wait. Otherwise, nodes won't boot properly
        //Thread.Sleep(120000);

        // Set up end-point nodes
        L1M1HandlerHelper.SetUpPCs(new VPC[2] { PC1, PC2 }, NetsPrefix);
        // Set-up routers
        L1M1HandlerHelper.SetUpRouters(Routers, NetsPrefix);
        // Set the routing tables signs
        for (int i = 0; i < Routers.Length; i++)
            L1M1HandlerHelper.SetRoutingTables(Routers[i].RoutingTable, NetTables[i]);
        // IP of the PC2
        DestinationIP.text = $"192.168.{ NetsPrefix[NetsPrefix.Length - 1]}/24";
        StartCoroutine("Timer");
    }

    void Update() {
        // We update the counter
        Counter.text = time.ToString();
    }

    //Simple Coroutine
    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            time++;
        }
    }

    private static class L1M1HandlerHelper {

        public static void SetUpPCs(VPC[] PCs, ushort[] NetsPrefix) {
            PCs[0].SetIP(
                IP: $"192.168.{NetsPrefix[0]}.11", gateway: $"192.168.{NetsPrefix[0]}.1"
            );
            PCs[1].SetIP(
                IP: $"192.168.{NetsPrefix[NetsPrefix.Length - 1]}.11", gateway: $"192.168.{NetsPrefix[NetsPrefix.Length - 1]}.2"
            );
        }

        public static void SetUpRouters(OpenWRT[] Routers, ushort[] NetsPrefix) {
            Task[] tasks = new Task[5]
            {
                Task.Factory.StartNew(() => SetUpR1(Routers[0], NetsPrefix)),
                Task.Factory.StartNew(() => SetUpR2(Routers[1], NetsPrefix)),
                Task.Factory.StartNew(() => SetUpR3(Routers[2], NetsPrefix)),
                Task.Factory.StartNew(() => SetUpR4(Routers[3], NetsPrefix)),
                Task.Factory.StartNew(() => SetUpR5(Routers[4], NetsPrefix))
            };
            Task.WaitAll(tasks);
        }

        private static void SetUpR1(OpenWRT R, ushort[] NetsPrefix) {
            // Net interfaces
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[0]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[2]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[1]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            R.DisableFirewall();
            // Routes
            R.SetRoute(destination: $"192.168.{NetsPrefix[7]}.0", gateway: $"192.168.{NetsPrefix[2]}.3", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[5]}.0", gateway: $"192.168.{NetsPrefix[2]}.3", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[3]}.0", gateway: $"192.168.{NetsPrefix[1]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[4]}.0", gateway: $"192.168.{NetsPrefix[2]}.3", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[6]}.0", gateway: $"192.168.{NetsPrefix[1]}.1", netmask: "255.255.255.0");
        }

        private static void SetUpR2(OpenWRT R, ushort[] NetsPrefix)
        {
            // Net interfaces
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[1]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[3]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[4]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            R.DisableFirewall();
            // Routes
            R.SetRoute(destination: $"192.168.{NetsPrefix[0]}.0", gateway: $"192.168.{NetsPrefix[1]}.3", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[2]}.0", gateway: $"192.168.{NetsPrefix[4]}.2", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[5]}.0", gateway: $"192.168.{NetsPrefix[4]}.2", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[6]}.0", gateway: $"192.168.{NetsPrefix[3]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[7]}.0", gateway: $"192.168.{NetsPrefix[3]}.1", netmask: "255.255.255.0");
        }

        private static void SetUpR3(OpenWRT R, ushort[] NetsPrefix)
        {
            // Net interfaces
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[5]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[4]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[2]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            R.DisableFirewall();
            // Routes
            R.SetRoute(destination: $"192.168.{NetsPrefix[7]}.0", gateway: $"192.168.{NetsPrefix[5]}.3", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[0]}.0", gateway: $"192.168.{NetsPrefix[2]}.2", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[1]}.0", gateway: $"192.168.{NetsPrefix[2]}.2", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[3]}.0", gateway: $"192.168.{NetsPrefix[4]}.3", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[6]}.0", gateway: $"192.168.{NetsPrefix[5]}.3", netmask: "255.255.255.0");
        }

        private static void SetUpR4(OpenWRT R, ushort[] NetsPrefix)
        {
            // Net interfaces
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[3]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[6]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            R.DisableFirewall();
            // Routes
            R.SetRoute(destination: $"192.168.{NetsPrefix[0]}.0", gateway: $"192.168.{NetsPrefix[3]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[1]}.0", gateway: $"192.168.{NetsPrefix[3]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[2]}.0", gateway: $"192.168.{NetsPrefix[3]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[4]}.0", gateway: $"192.168.{NetsPrefix[3]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[5]}.0", gateway: $"192.168.{NetsPrefix[6]}.2", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[7]}.0", gateway: $"192.168.{NetsPrefix[6]}.2", netmask: "255.255.255.0");
        }

        private static void SetUpR5(OpenWRT R, ushort[] NetsPrefix)
        {
            // Net interfaces
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[6]}.1", interfaceNumber: 1, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[7]}.2", interfaceNumber: 2, netmask: "255.255.255.0");
            R.ActivateInterface(IP: $"192.168.{NetsPrefix[5]}.3", interfaceNumber: 3, netmask: "255.255.255.0");
            R.DisableFirewall();
            // Routes
            R.SetRoute(destination: $"192.168.{NetsPrefix[0]}.0", gateway: $"192.168.{NetsPrefix[5]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[1]}.0", gateway: $"192.168.{NetsPrefix[5]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[2]}.0", gateway: $"192.168.{NetsPrefix[5]}.1", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[3]}.0", gateway: $"192.168.{NetsPrefix[6]}.2", netmask: "255.255.255.0");
            R.SetRoute(destination: $"192.168.{NetsPrefix[4]}.0", gateway: $"192.168.{NetsPrefix[5]}.1", netmask: "255.255.255.0");
        }

        public static void SetRoutingTables(RoutingTable RoutTab, GameObject TextToReplicate) {

                float positionOffset = 0.2f;
                // Assign the sign to a new variable
                var newTextWrapper = TextToReplicate;
                foreach (RoutingTable.RoutingTableRow route in RoutTab.Routes)
                {
                    if (route.Iface.Contains("eth")) {
                        // Duplicate the canvas for the routing tables
                        newTextWrapper = Instantiate(newTextWrapper, newTextWrapper.transform);
                        // Move it down
                        newTextWrapper.transform.Translate(0, -positionOffset, 0);
                        // Write down the parameters into the sign
                        var dest = newTextWrapper.transform.GetChild(0).GetComponent<Text>();
                        dest.text = route.Destination;
                        var gate = newTextWrapper.transform.GetChild(1).GetComponent<Text>();
                        gate.text = route.Gateway;
                        var netmask = newTextWrapper.transform.GetChild(2).GetComponent<Text>();
                        netmask.text = route.Netmask;
                        var eth = newTextWrapper.transform.GetChild(3).GetComponent<Text>();
                        eth.text = route.Iface;
                    }
                }
            }

        }

}
