using RingMenuJJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="add RingButton", fileName = "New RingButton")]
public class RingButton_EditorMode : ScriptableObject
{    //https://www.youtube.com/watch?v=aPXvoWVabPY&t=84s
    public bool visible;

    public Color button_color;

    public string label;
    public bool label_show;

    public Texture2D icon;
    public bool icon_show;

    public int ring_index;
    public float button_index_on_ring;
    public int button_index_on_ring_int;





    public Font label_font;
    public FontStyle label_fontStyle;
    public bool label_resizeTextForBestFit = false;
    public int label_fontSize;
    public Color label_color;

    public string nom;
    internal RingButton_Manager ringButtonManager;
}
