using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GNS3sharp;

public class GNS3Handler : MonoBehaviour {

    public static GNS3Handler Instance { get; private set; } = null;
    public GNS3sharp.GNS3sharp projectHandler = new GNS3sharp.GNS3sharp("575a3cf3-7ff3-4924-8fed-9df4e6fef351");

    //Awake is always called before any Start functions
    void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this)  Destroy(gameObject);
    }
}
