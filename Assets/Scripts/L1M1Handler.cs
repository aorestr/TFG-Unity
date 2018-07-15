using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1M1Handler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GNS3Handler.Instance.projectHandler.StartProject();
	}
}
