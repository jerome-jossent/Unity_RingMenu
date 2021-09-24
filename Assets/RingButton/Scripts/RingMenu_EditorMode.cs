using RingMenuJJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RingMenu_EditorMode : MonoBehaviour
{
    [Header("Disposition")]
    [Range(0, 360)] public float angle_initial;
    public bool sens_horaire = true;

    [Range(0, 1)] public float marge = 0.1f;
    [Range(0, 100)] public float R0_R = 50;
    [Range(0, 100)] public float R1_R = 50;
    [Range(0, 100)] public float R2_R = 50;
    [Range(0, 100)] public float R3_R = 50;
    [Range(0, 100)] public float R4_R = 50;

    [Space]
    [Header("Boutons")]
    public RingButton_EditorMode[] Boutons_Ring0;
    public RingButton_EditorMode[] Boutons_Ring1;
    public RingButton_EditorMode[] Boutons_Ring2;
    public RingButton_EditorMode[] Boutons_Ring3;
    public RingButton_EditorMode[] Boutons_Ring4;

    [Space]
    [Header("Couleurs")]
    public bool setDefaultColors;
    public bool randomColors;
    public bool distributeColors;

    RingMenu_Manager ringMenu_Manager;
  public  GameObject ringMenu;

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
        Debug.Log("dr");
        Dictionary<int, Anneau> anneaux = new Dictionary<int, Anneau>();

        List<RingButton_EditorMode[]> rings = new List<RingButton_EditorMode[]>();
        rings.Add(Boutons_Ring0);
        rings.Add(Boutons_Ring1);
        rings.Add(Boutons_Ring2);
        rings.Add(Boutons_Ring3);
        rings.Add(Boutons_Ring4);

        List<float> rayons = Anneau.GetRayons(new List<float> { R0_R, R1_R, R2_R, R3_R, R4_R });

        int nbrbuttons = 0;
        for (int i = 0; i < rings.Count; i++)
        {
            RingButton_EditorMode[] ring = rings[i];
            foreach (RingButton_EditorMode item in ring)
            {
                if (item == null || !item.visible)
                    continue;

                if (!anneaux.ContainsKey(i))
                    anneaux.Add(i, new Anneau(i, rayons, marge));

                while (anneaux[i].butons_on_ring.ContainsKey(item.button_index_on_ring))
                    item.button_index_on_ring++;
                anneaux[i].butons_on_ring.Add(item.button_index_on_ring, item);
                nbrbuttons++;
            }
        }

        Dictionary<string, RingButton_EditorMode> touslesboutonsaffiches = new Dictionary<string, RingButton_EditorMode>();
        foreach (Anneau ring in anneaux.Values)
        {
            ring.Sort();
            foreach (RingButton_EditorMode item in ring.butons_on_ring_sorted.Values)
                touslesboutonsaffiches.Add(item.name, item);
        }

        //foreach (Anneau ring in anneaux.Values)
        //    foreach (var item in ring.butons_on_ring_sorted)
        //    {
        //        print("ring[" + ring.index + "] button[" + item.Value.button_index_on_ring_int + "]");
        //    }
        Color[] colors = null;
        if (setDefaultColors)
            colors = SetColors(nbrbuttons, randomColors, distributeColors, 0.8f);

        Font arial = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        foreach (Anneau ring in anneaux.Values)
            foreach (RingButton_EditorMode item in ring.butons_on_ring_sorted.Values)
            {
                if (setDefaultColors)
                    item.button_color = colors[item.button_index_on_ring_int];

                item.label_font = arial;
            }

        if (setDefaultColors)
            setDefaultColors = false;

        if (ringMenu_Manager != null)
        {
            //ringMenu_Manager._OnSelected -= _RingMenu_Manager__OnSelected;
            //ringMenu_Manager._OnEnter -= _RingMenu_Manager__OnEnter;
            //ringMenu_Manager._OnExit -= _RingMenu_Manager__OnExit;
            DestroyImmediate(ringMenu);
        }

        //C'est parti :
        ringMenu = RingMenu._DrawRingMenu(anneaux, angle_initial, sens_horaire);
        ringMenu.name = "menu";
        ringMenu.transform.parent = gameObject.transform;
        ringMenu.transform.localPosition = Vector3.zero;
        ringMenu_Manager = ringMenu.GetComponent<RingMenu_Manager>();

        foreach (var item in touslesboutonsaffiches.Values)
            item.ringButtonManager._SetNormalColor(item.ringButtonManager.gameObject);

        //ringMenu_Manager._OnSelected += _RingMenu_Manager__OnSelected;
        //ringMenu_Manager._OnEnter += _RingMenu_Manager__OnEnter;
        //ringMenu_Manager._OnExit += _RingMenu_Manager__OnExit;
    }

    //public void _RingMenu_Manager__OnExit(object sender, EventArgs e)
    //{
    //    RingButton_Manager rbm = (RingButton_Manager)sender;
    //    Debug.Log(rbm.gameObject.name + " [EXIT]");
    //}

    //public void _RingMenu_Manager__OnEnter(object sender, EventArgs e)
    //{
    //    RingButton_Manager rbm = (RingButton_Manager)sender;
    //    Debug.Log(rbm.gameObject.name + " [ENTER]");
    //}

    //public void _RingMenu_Manager__OnSelected(object sender, EventArgs e)
    //{
    //    RingButton_Manager rbm = (RingButton_Manager)sender;
    //    Debug.Log(rbm.gameObject.name + " [SELECTED]");
    //}








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
