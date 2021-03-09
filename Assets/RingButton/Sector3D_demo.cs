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

    [SerializeField] TMPro.TMP_InputField _if_obj;
    [SerializeField] TMPro.TMP_InputField _if_mtl;
    [SerializeField] TMPro.TMP_InputField _if_fbx;
    [SerializeField] TMPro.TMP_Text _txt_obj;
    [SerializeField] TMPro.TMP_Text _txt_mtl;
    [SerializeField] TMPro.TMP_Text _txt_fbx;

    [SerializeField] UnityEngine.UI.InputField _if_obj2;
    [SerializeField] UnityEngine.UI.InputField _if_mtl2;
    [SerializeField] UnityEngine.UI.InputField _if_fbx2;


    [SerializeField] ToHTML tohtml;

    [SerializeField] GameObject Spawn;

    GameObject ringMenu;
    RingMenu_Manager ringMenu_Manager;

    Color[] colors;
    UnityEngine.Object[] textures;

    void Start()
    {
        colors = SetColors(20, false, true, 0.8f);

        textures = Resources.LoadAll("Emoticons", typeof(Texture2D));

        _SliderValueChange();
    }

    Color[] SetColors(int nbrcolor, bool random, bool distribute, float alpha = 1f)
    {
        Color[] colors = new Color[nbrcolor];
        for (int i = 0; i < colors.Length; i++)
        {
            if (random)
                colors[i] = UnityEngine.Random.ColorHSV(0f, 1f * i / nbrcolor, 1f, 1f, 0.5f, 1f, 0.8f, 0.8f);
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

    public void _SliderValueChange()
    {
        if (ringMenu_Manager != null)
        {
            ringMenu_Manager._OnSelected -= RingMenu_Manager__OnSelected;
            ringMenu_Manager._OnEnter -= RingMenu_Manager__OnEnter;
            ringMenu_Manager._OnExit -= RingMenu_Manager__OnExit;
        }

        DestroyImmediate(ringMenu);

        int R0_B = (int)_sld_anneau0_btn.value;
        int R1_B = (int)_sld_anneau1_btn.value;
        int R2_B = (int)_sld_anneau2_btn.value;
        int R3_B = (int)_sld_anneau3_btn.value;
        int R4_B = (int)_sld_anneau4_btn.value;

        int nbrButtons = R0_B + R1_B + R2_B + R3_B + R4_B;
        _txt_btns.text = nbrButtons + " boutons";

        colors = SetColors(nbrButtons);

        float R0_R = _sld_anneau0_taille.value;
        float R1_R = _sld_anneau1_taille.value;
        float R2_R = _sld_anneau2_taille.value;
        float R3_R = _sld_anneau3_taille.value;
        float R4_R = _sld_anneau4_taille.value;
        float marge = _sld_marge.value;

        try
        {
            List<int> btns = new List<int> { R0_B, R1_B, R2_B, R3_B, R4_B };
            List<float> epaisseurs = new List<float> { R0_R, R1_R, R2_R, R3_R, R4_R };
            List<Color[]> couleurs = new List<Color[]> { colors, colors, colors, colors, colors };
            ringMenu = RingMenu._DrawRingMenu(btns, epaisseurs, marge, couleurs, null);
            ringMenu_Manager = ringMenu.GetComponent<RingMenu_Manager>();

            ringMenu_Manager._OnSelected += RingMenu_Manager__OnSelected;
            ringMenu_Manager._OnEnter += RingMenu_Manager__OnEnter;
            ringMenu_Manager._OnExit += RingMenu_Manager__OnExit;

            _txt_debug.text = "";
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogError(ex);
#else
            _txt_debug.text = ex.Message + "\n\n" + ex.StackTrace;
#endif
        }

        ringMenu.transform.position = Spawn.transform.position;
        ringMenu.transform.rotation = Spawn.transform.rotation;
        ringMenu.transform.localScale = Spawn.transform.localScale;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        ringMenu_Manager._InteractionManager(ray, Input.GetMouseButtonDown(0));
    }

    private void RingMenu_Manager__OnExit(object sender, EventArgs e)
    {
        _txt_btn.text = "";
    }

    private void RingMenu_Manager__OnEnter(object sender, EventArgs e)
    {
        RingButton_Manager rbm = (RingButton_Manager)sender;
        _txt_btn.text = rbm._name;
        _txt_btn.fontStyle = FontStyle.Normal;
        _txt_btn.color = Color.black;
    }

    private void RingMenu_Manager__OnSelected(object sender, EventArgs e)
    {
        RingButton_Manager rbm = (RingButton_Manager)sender;
        _txt_btn.text = rbm._name;
        _txt_btn.fontStyle = FontStyle.Bold;
        _txt_btn.color = Color.red;
    }


    public void _ExportToOBJMTL()
    {
        //OBJ & MTL
        MeshExporter.ObjExporter.OBJ_MTL_TXT data = MeshExporter.ObjExporter.MeshToString(ringMenu, ringMenu.name);
        tohtml._String_TO_File(data.objfilename, data.obj);
        tohtml._String_TO_File(data.mtlfilename, data.mtl);
    }

    public void _ExportToFBX()
    {
        //FBX
        string path = ringMenu.name + ".fbx";
        string fbxfile = UnityFBXExporter.FBXExporter.MeshToString(ringMenu, path, false, false);
        tohtml._String_TO_File(path, fbxfile);
    }
}