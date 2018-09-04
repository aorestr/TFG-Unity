using UnityEngine;
using GNS3sharp;

public class GNS3Handler : MonoBehaviour {

    public static GNS3Handler Instance { get; private set; } = null;
    public GNS3sharp.GNS3sharp projectHandler;

    // Awake is always called before any Start function
    void Awake() {
        if (Instance == null) {
            Instance = this;
            // If you want to use a GNS3 located on the local machine
            /*
            this.projectHandler = new GNS3sharp.GNS3sharp(
                GNS3sharp.ServerProjects.GetProjectIDByName("GameNet")
            );
            */
            // If you use another machine with IP 192.168.1.157
            this.projectHandler = new GNS3sharp.GNS3sharp(
                ServerProjects.GetProjectIDByName("GameNet", host: "192.168.1.157"),
                _host: "192.168.1.157"
            );
        }
        else if (Instance != this)  Destroy(gameObject);
    }

}
