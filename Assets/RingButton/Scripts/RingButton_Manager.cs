using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RingMenuJJ
{
    public class RingButton_Manager : MonoBehaviour
    {
        //internal string _name;
        internal int _ring_index;
        internal int _index;
        public Color _couleurfonce;
        public Color _couleurhighlight;
        public Color _couleur;
        GameObject btn;
        internal GameObject _icone;

        // Event delegates triggered on click.
        public UnityEngine.UI.Button.ButtonClickedEvent _OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        public UnityEngine.UI.Button.ButtonClickedEvent _OnEnter = new UnityEngine.UI.Button.ButtonClickedEvent();
        public UnityEngine.UI.Button.ButtonClickedEvent _OnExit = new UnityEngine.UI.Button.ButtonClickedEvent();

        private void Start()
        {
            btn = gameObject;
            _SetNormalColor();
        }

        public void _SetColors(Color absolutecolor)
        {
            _couleurhighlight = absolutecolor;
            _couleur = ColorIntensity(absolutecolor, 0.8f);
            _couleurfonce = ColorIntensity(absolutecolor, 0.64f);
        }

        static public Color ColorIntensity(Color origine, float factor)
        {
            Color.RGBToHSV(origine, out float h, out float s, out float v);
            Color color = Color.HSVToRGB(h, s, v * factor);
            color.a = origine.a;
            return color;
        }

        internal void _SetNormalColor()
        {
            MaterialSetColor.Colorier(btn, _couleur);
        }

        internal void _SetNormalColor(GameObject gameObject)
        {
            MaterialSetColor.Colorier(gameObject, _couleur);
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
}