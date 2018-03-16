using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

	public enum ColorNames
    {
        RED,
        BLUE,
        PURPLE
    }

    private static Hashtable colorValues = new Hashtable
    {
        {ColorNames.RED, new Color32(255, 84, 84, 255)},
        {ColorNames.BLUE, new Color32(0, 255, 255, 255)},
        {ColorNames.PURPLE, new Color32(255, 159, 248, 255)}
    };

    public static Color32 GetColorValue(ColorNames name)
    {
        return (Color32)colorValues[name];
    }
}
