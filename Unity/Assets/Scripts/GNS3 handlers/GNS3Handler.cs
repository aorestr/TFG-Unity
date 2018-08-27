using UnityEngine;

public class GNS3Handler : MonoBehaviour {

    public static GNS3Handler Instance { get; private set; } = null;
    public GNS3sharp.GNS3sharp projectHandler;

    // Awake is always called before any Start function
    void Awake() {
        if (Instance == null) {
            Instance = this;
            this.projectHandler = new GNS3sharp.GNS3sharp(
                GNS3sharp.ServerProjects.GetProjectIDByName("GameNet")
            );
        }
        else if (Instance != this)  Destroy(gameObject);
    }

}
