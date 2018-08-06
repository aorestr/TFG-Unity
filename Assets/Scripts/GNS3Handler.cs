using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GNS3sharp;

public class GNS3Handler : MonoBehaviour {

    public static GNS3Handler Instance { get; private set; } = null;
    public GNS3sharp.GNS3sharp projectHandler = new GNS3sharp.GNS3sharp("82754fb3-7027-4556-b79f-1da07506437e");

    //Awake is always called before any Start functions
    void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this)  Destroy(gameObject);
    }
}
