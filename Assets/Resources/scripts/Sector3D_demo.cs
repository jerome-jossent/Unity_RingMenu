using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector3D_demo : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Slider _sld_anneau0_btn;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau1_btn;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau2_btn;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau3_btn;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau4_btn;

    [SerializeField] UnityEngine.UI.Slider _sld_anneau0_taille;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau1_taille;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau2_taille;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau3_taille;
    [SerializeField] UnityEngine.UI.Slider _sld_anneau4_taille;

    [SerializeField] UnityEngine.UI.Slider _sld_marge;

    [SerializeField] UnityEngine.UI.Text _txt_btns;
    [SerializeField] UnityEngine.UI.Text _txt_btn;
    [SerializeField] UnityEngine.UI.Text _txt_debug;

    [SerializeField] GameObject Spawn;

    GameObject ringMenu;
    RingBouton rb_previsous = null;
    Color[] colors;
    int btn_index_onthisRing;
    Dictionary<string, RingBouton> dico;
    UnityEngine.Object[] textures;

    void Start()
    {
        colors = DistributeColors(20, 0.8f);
        //for (int i = 0; i < 20; i++)
        //    colors[i] = new Color(0.9f, 0.9f, 0.9f, 0.8f);

        textures = Resources.LoadAll("Emoticons", typeof(Texture2D));

        _SliderValueChange();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log(hit.transform.name);
            _txt_btn.text = hit.transform.name;
            RingBouton rb = dico[hit.transform.name];
            if (rb != rb_previsous)
            {
                if (rb_previsous != null)
                    rb_previsous._SetNormalColor();

                if (rb != null)
                    rb._SetHighlightColor();
                rb_previsous = rb;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (rb != null)
                    rb._SetSelectedColor();
            }
        }
        else
        {
            _txt_btn.text = "";
            if (rb_previsous != null)
                rb_previsous._SetNormalColor();
            rb_previsous = null;
        }
    }

    Color[] DistributeColors(int nbrcolor, float alpha = 1f)
    {
        Color[] colors = new Color[nbrcolor];
        for (int i = 0; i < colors.Length; i++)
        {
            //colors[i] = UnityEngine.Random.ColorHSV(0f, 1f * i / nbrcolor, 1f, 1f, 0.5f, 1f, 0.8f, 0.8f);
            colors[i] = Color.HSVToRGB(1f * i / nbrcolor, 1f, 1f);
            colors[i].a = alpha;
        }
        return colors;

        //distribuer les couleurs : échanger 1 couleur sur 2
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
        return colors;
    }

    public void _SliderValueChange()
    {
        Destroy(ringMenu);
        ringMenu = new GameObject();

        int R0_B = (int)_sld_anneau0_btn.value;
        int R1_B = (int)_sld_anneau1_btn.value;
        int R2_B = (int)_sld_anneau2_btn.value;
        int R3_B = (int)_sld_anneau3_btn.value;
        int R4_B = (int)_sld_anneau4_btn.value;
        _txt_btns.text = (R0_B + R1_B + R2_B + R3_B + R4_B) + " boutons";

        float R0_R = _sld_anneau0_taille.value;
        float R1_R = _sld_anneau1_taille.value;
        float R2_R = _sld_anneau2_taille.value;
        float R3_R = _sld_anneau3_taille.value;
        float R4_R = _sld_anneau4_taille.value;

        float marge = _sld_marge.value; ;
        dico = new Dictionary<string, RingBouton>();

        try
        {
            GameObject a0 = DrawRing(0, R0_R, R0_R, R0_B, marge);
            GameObject a1 = DrawRing(1, R0_R + R1_R, R1_R, R1_B, marge);
            GameObject a2 = DrawRing(2, R0_R + R1_R + R2_R, R2_R, R2_B, marge);
            GameObject a3 = DrawRing(3, R0_R + R1_R + R2_R + R3_R, R3_R, R3_B, marge);
            GameObject a4 = DrawRing(4, R0_R + R1_R + R2_R + R3_R + R4_R, R4_R, R4_B, marge);
            a0.transform.parent = ringMenu.transform;
            a1.transform.parent = ringMenu.transform;
            a2.transform.parent = ringMenu.transform;
            a3.transform.parent = ringMenu.transform;
            a4.transform.parent = ringMenu.transform;
            _txt_debug.text = "";
        }
        catch (Exception ex)
        {
            _txt_debug.text = ex.Message + "\n\n" + ex.StackTrace;
        }

        ringMenu.transform.position = Spawn.transform.position;
        ringMenu.transform.rotation = Spawn.transform.rotation;
        ringMenu.transform.localScale = Spawn.transform.localScale;
    }

    GameObject DrawRing(int ring_index, float r_ext, float epaisseur, int nbrboutons, float marge)
    {
        btn_index_onthisRing = 0;
        GameObject go = new GameObject();
        float r_int = r_ext - epaisseur;

        float angle_ouverture_deg = (float)360 / nbrboutons;
        float angle_position_deg_init = angle_ouverture_deg / 2;

        for (int i = 0; i < nbrboutons; i++)
        {
            //Button b = new Button();
            float angle_position_deg = angle_position_deg_init + i * angle_ouverture_deg;

            GameObject btn = DrawButton(r_ext,
                                r_int,
                                angle_ouverture_deg,
                                angle_position_deg,
                                marge);
            btn.name = "ring_" + ring_index + "_btn_" + i;
            btn.transform.parent = go.transform;


            Texture texture = (Texture)textures[btn_index_onthisRing];
            //Resources.Load<Texture>("Emoticons/anger") as Texture;

            GameObject icn = DrawIcon(btn, texture);
            icn.transform.parent = go.transform;

            RingBouton rb = btn.AddComponent<RingBouton>();
            rb._name = btn.name;
            rb._ring_index = ring_index;
            rb._index = i;
            rb._SetColors(colors[btn_index_onthisRing]);
            rb._icone = icn;

            dico.Add(btn.name, rb);

            btn_index_onthisRing++;
        }

        go.name = "Ring_" + ring_index;
        return go;
    }

    GameObject DrawButton(float r_ext, float r_int, float angle_ouverture_deg, float angle_position_deg, float marge)
    {
        GameObject secteur = Sector3D.CreateObject(r_int, r_ext, angle_position_deg, angle_position_deg + angle_ouverture_deg, marge, name: "A0");
        return secteur;
    }

    GameObject DrawIcon(GameObject secteur, Texture icone)
    {
        // A sphere that fully encloses the bounding box.
        //https://docs.unity3d.com/ScriptReference/Renderer-bounds.html
        Renderer rend = secteur.GetComponent<Renderer>();
        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;
        radius *= Mathf.Pow(0.5f,0.5f);

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = new Material(Shader.Find("Unlit/TransparentColored"));
        mat.mainTexture = icone;
        quad.GetComponent<Renderer>().material = mat;

        quad.transform.Rotate(90, 0, 0);
        quad.transform.localScale = new Vector3(radius, radius);
        quad.transform.position = center + Vector3.up;

        return quad;
    }
}