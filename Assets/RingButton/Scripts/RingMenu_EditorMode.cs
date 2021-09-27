using RingMenuJJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RingMenu_EditorMode : MonoBehaviour
{
    public int anneau_index = 0;
    [Range(0, 360)] public float angle_initial;
    public bool sens_horaire = true;

    [Range(0, 1)] public float marge = 0.1f;
    [Range(0, 100)] public float r_interne = 50;
    [Range(0, 100)] public float r_externe = 100;

    [UnityExtensions.ReorderableList(elementsAreSubassets = true)]
    public RingButton_EditorMode[] Boutons;

    public bool setDefaultColors;
    public bool randomColors;
    public bool distributeColors;

    RingMenu_Manager ringMenu_Manager;
    public GameObject ringMenu;

    bool aValueInInspectorHasChanged;
    void OnValidate() { aValueInInspectorHasChanged = true; }

    void Start()
    {
        ringMenu = gameObject.transform.Find("menu").gameObject;
        ringMenu_Manager = gameObject.GetComponentInChildren<RingMenu_Manager>();
    }

    void Update()
    {
        if (aValueInInspectorHasChanged)
        {
            Draw();
            aValueInInspectorHasChanged = false;
        }
    }

    void Draw()
    {
        if (Boutons.Length == 0)
        {
            //reset
            DestroyImmediate(ringMenu);
            return;
        }

        int nbrbuttons = 0;
        Anneau a = new Anneau(anneau_index, r_interne, r_externe, marge);
        foreach (RingButton_EditorMode item in Boutons)
        {
            if (item == null || !item.visible)
                continue;
            item.button_index_on_ring_int = nbrbuttons;
            item.name = nbrbuttons.ToString();            
            a.butons_on_ring.Add(nbrbuttons, item);
            nbrbuttons++;
        }

        if (nbrbuttons == 0)
            return;

        Color[] colors = null;
        if (setDefaultColors)
            colors = SetColors(nbrbuttons, randomColors, distributeColors, 0.8f);

        Font arial = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        foreach (RingButton_EditorMode item in a.butons_on_ring.Values)
        {
            if (setDefaultColors)
                item.button_color = colors[item.button_index_on_ring_int];

            item.label.label_font = arial;
        }

        if (setDefaultColors)
            setDefaultColors = false;

        if (ringMenu_Manager != null)
        {
            DestroyImmediate(ringMenu);
        }

        Dictionary<int, Anneau> anneaux = new Dictionary<int, Anneau>();
        anneaux.Add(a.index, a);

        //C'est parti :
        ringMenu = RingMenu._DrawRingMenu(anneaux, angle_initial, sens_horaire);
        ringMenu.name = "menu";
        ringMenu.transform.parent = gameObject.transform;
        ringMenu.transform.localPosition = Vector3.zero;
        ringMenu_Manager = ringMenu.GetComponent<RingMenu_Manager>();        

        foreach (var item in a.butons_on_ring.Values)
            item.ringButtonManager._SetNormalColor(item.ringButtonManager.gameObject);
    }

    Color[] SetColors(int nbrcolor, bool random, bool distribute, float alpha = 1f)
    {
        Color[] colors = new Color[nbrcolor];
        for (int i = 0; i < colors.Length; i++)
        {
            if (random)
                //colors[i] = UnityEngine.Random.ColorHSV(0f, 1f * i / nbrcolor, 1f, 1f, 0.5f, 1f, 0.8f, 0.8f);
                colors[i] = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 0.8f, 0.8f);
            else
            {
                colors[i] = Color.HSVToRGB(1f * i / nbrcolor, 1f, 1f);
                colors[i].a = alpha;
            }
        }

        //distribuer les couleurs : échanger 1 couleur sur 2
        if (distribute)
        {
            for (int i = 0; i < colors.Length / 2; i++)
            {
                if (i % 2 == 0)
                {
                    Color temp = colors[i];
                    int newIndex = colors.Length / 2 + i;
                    if (newIndex > colors.Length - 1)
                        newIndex -= colors.Length;
                    colors[i] = colors[newIndex];
                    colors[newIndex] = temp;
                }
            }
        }

        return colors;
    }
}