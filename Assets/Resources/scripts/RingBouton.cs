using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBouton : MonoBehaviour
{
    internal string _name;
    internal int _ring_index;
    internal int _index;

    public Color _couleurfonce;
    public Color _couleurhighlight;
    public Color _couleur;

    GameObject btn;

    private void Start()
    {
        btn = gameObject;
        _SetNormalColor();
    }
    public void _SetColors(Color absolutecolor)
    {
        _couleurhighlight = absolutecolor;
        float fctr = 0.8f;
        Color.RGBToHSV(_couleurhighlight, out float h, out float s, out float v);

        _couleur = Color.HSVToRGB(h, s, v * fctr); 
        _couleur.a = absolutecolor.a;

        _couleurfonce = Color.HSVToRGB(h, s, v * fctr * fctr);
        _couleurfonce.a = absolutecolor.a;
    }

    internal void _SetNormalColor()
    {
        MaterialSetColor.Colorier(btn, _couleur);
    }

    internal void _SetHighlightColor()
    {
        MaterialSetColor.Colorier(btn, _couleurhighlight);
    }

    internal void _SetSelectedColor()
    {
        MaterialSetColor.Colorier(btn, _couleurfonce);
    }
}
