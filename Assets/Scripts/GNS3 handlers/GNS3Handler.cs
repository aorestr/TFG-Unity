using UnityEngine;
using System.Collections.Generic;

public class GNS3Handler : MonoBehaviour {

    public static GNS3Handler Instance { get; private set; } = null;
    public GNS3sharp.GNS3sharp projectHandler = new GNS3sharp.GNS3sharp(
        GNS3sharp.ServerProjects.GetProjectIDByName("GameNet")
    );

    //Awake is always called before any Start functions
    void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this)  Destroy(gameObject);
    }

    // Allow to randomize a several links on a project. It is useful
    // in /24 nets specially, when the third byte is something like 10, 20, 60...
    public static ushort[] RandomizeNets(ushort numLinks) {
        // Result list
        List<ushort> tempLinks = new List<ushort>(numLinks);
        // Initial list
        List<ushort> initialList = new List<ushort>(numLinks);
        // Random variable
        System.Random rnd = new System.Random();

        for (ushort i = 10; i <= numLinks * 10; i += 10) initialList.Add(i);
        for (ushort i = 0; i < numLinks; i++) {
            // Pick a random net number (10, 20, 30...) from the list
            var randIdx = rnd.Next(0, initialList.Count);
            tempLinks.Add(initialList[randIdx]);
            initialList.RemoveAt(randIdx);
        }
        return tempLinks.ToArray();
    }

}
