using System.Collections.Generic;

public class Constants
{
    public const string saveDirectory = "/Saved_low_test_Depth/all/";
    public const int maxNumberOfObjects = 13;
    public const int scenesPerObjectsKoeff = 20;
    public static readonly Dictionary<int, string[]> shaders = new Dictionary<int, string[]>()
    {
        { 0, new string[2]{"Normal", "MyShaders/NormalImageEffect"} },
        { 1, new string[2]{"Depth", "MyShaders/DepthImageEffect"} },
        { 2, new string[2]{"Regular", "MyShaders/RegularImageEffect"} }
    };
}
