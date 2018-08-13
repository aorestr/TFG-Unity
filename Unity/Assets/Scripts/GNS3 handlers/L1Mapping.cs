using System.Linq;
using System.Collections.Generic;

public struct L1Mapping {

    // Random seed
    private static readonly System.Random rnd = new System.Random();

    /////////////////* Minigame 1 *////////////////////

    // Nodes for the first minigame
    public static string[] NodeNamesM1 = new string[] {
        "[VPC]L1PC1","[VPC]L1PC2","[OPENWRT]L1R1",
        "[OPENWRT]L1R2","[OPENWRT]L1R3","[OPENWRT]L1R4",
        "[OPENWRT]L1R5"
    };

    // List the connections between the nodes
    public static Dictionary<string, Dictionary<ushort, string>> NextNodeM1 =
        new Dictionary<string, Dictionary<ushort, string>>()
        {
            { "PC1",
                new Dictionary<ushort, string>(){
                    {0, "R1"}
                }
            },
            { "R1",
                new Dictionary<ushort, string>(){
                    {1, "PC1"}, {2, "R3"}, {3, "R2"}
                }
            },
            { "R2",
                new Dictionary<ushort, string>(){
                    {1, "R1"}, {2, "R4"}, {3, "R3"}
                }
            },
            { "R3",
                new Dictionary<ushort, string>(){
                    {1, "R5"}, {2, "R2"}, {3, "R1"}
                }
            },
            { "R4",
                new Dictionary<ushort, string>(){
                    {1, "R2"}, {2, "R5"}
                }
            },
            { "R5",
                new Dictionary<ushort, string>(){
                    {1, "R4"}, {2, "PC2"}, {3, "R3"}
                }
            }
        };

    // Set the place where to show up at every platform
    public static Dictionary<string, float[]> NodeRespawnM1 = new Dictionary<string, float[]>() {
        {"PC1", new float[2]{ -9.694201f, -1.0057f } },
        {"R1", new float[2]{ -13.2f, 5.3f } },
        {"R2", new float[2]{ 5.4f, 12.31f } },
        {"R3", new float[2]{ 15.01f, 6.97f } },
        {"R4", new float[2]{ 10f, -7f } },
        {"R5", new float[2]{ 32.4f, 0.2f } },
        {"PC2", new float[2]{ 64.05f, 8.06f } }
    };

    // Random order for doors
    public static Dictionary<string, ushort[]> DoorsOrderM1 = new Dictionary<string, ushort[]>()
    {
        { "PC1", new ushort[1]{ 0 } },
        { "R1", new ushort[3]{ 1, 2, 3 }.OrderBy(x => rnd.Next()).ToArray() },
        { "R2", new ushort[3]{ 1, 2, 3 }.OrderBy(x => rnd.Next()).ToArray() },
        { "R3", new ushort[3]{ 1, 2, 3 }.OrderBy(x => rnd.Next()).ToArray() },
        { "R4", new ushort[2]{ 1, 2 }.OrderBy(x => rnd.Next()).ToArray() },
        { "R5", new ushort[3]{ 1, 2, 3 }.OrderBy(x => rnd.Next()).ToArray() }
    };

    // Allow to randomize a several links on a project. It is useful
    // in /24 nets specially, when the third byte is something like 10, 20, 60...
    public static ushort[] RandomizeNets(ushort numLinks)
    {
        // Result list
        List<ushort> tempLinks = new List<ushort>(numLinks);
        // Initial list
        List<ushort> initialList = new List<ushort>(numLinks);
        // Random variable
        System.Random rnd = new System.Random();

        for (ushort i = 10; i <= numLinks * 10; i += 10) initialList.Add(i);
        for (ushort i = 0; i < numLinks; i++)
        {
            // Pick a random net number (10, 20, 30...) from the list
            var randIdx = rnd.Next(0, initialList.Count);
            tempLinks.Add(initialList[randIdx]);
            initialList.RemoveAt(randIdx);
        }
        return tempLinks.ToArray();
    }
}
