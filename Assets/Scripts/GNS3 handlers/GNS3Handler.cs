using UnityEngine;

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

}
