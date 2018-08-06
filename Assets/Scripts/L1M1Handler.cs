using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GNS3sharp;

public class L1M1Handler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (string nodeName in L1NodesNames.M1) {
            GNS3Handler.Instance.projectHandler.StartNode(
                GNS3Handler.Instance.projectHandler.GetNodeByName(nodeName)
            );
        }
    }
}
