using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    static ColorPalette _instance;
    public static ColorPalette Instance { get { return _instance; } }

    public Color[] theme1;
    public Color[] ItemTextTheme;
    public Color[] theme3;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    Color[] GetColors()
    {
        return theme1;
    }

}
