using System.Collections;
using System.Collections.Generic;

namespace RingMenuJJ
{
    public class Bouton
    {
        public string label;
        public int index;

        public UnityEngine.Font label_font;
        public UnityEngine.FontStyle label_fontStyle;
        public bool label_resizeTextForBestFit = false;
        public int label_fontSize;
        public UnityEngine.Color label_color;

        public UnityEngine.Texture2D icone;
        internal string name;
    }
}