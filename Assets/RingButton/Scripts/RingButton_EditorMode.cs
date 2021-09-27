using RingMenuJJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RingButton_EditorMode
{
    public bool visible = true;
    public string name;
    [HideInInspector] public int button_index_on_ring_int;

    public Color button_color = Color.white;
    public Texture2D icon;
    public Label label = new Label();
    public Events events = new Events();

    internal RingButton_Manager ringButtonManager;
}

[Serializable]
public class Label
{
    public string label;
    public bool label_show = true;

    public Font label_font;
    public FontStyle label_fontStyle;
    public bool label_resizeTextForBestFit = false;
    public int label_fontSize;
    public Color label_color;
}

[Serializable]
public class Events
{
    public UnityEngine.UI.Button.ButtonClickedEvent _OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
    public UnityEngine.UI.Button.ButtonClickedEvent _OnEnter = new UnityEngine.UI.Button.ButtonClickedEvent();
    public UnityEngine.UI.Button.ButtonClickedEvent _OnExit = new UnityEngine.UI.Button.ButtonClickedEvent();
}