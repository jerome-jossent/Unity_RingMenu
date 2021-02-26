using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector3D_demo_fromList1 : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Slider _sld_btns;
    [SerializeField] UnityEngine.UI.Slider _sld_marge;

    [SerializeField] UnityEngine.UI.Text _txt_btns;
    [SerializeField] UnityEngine.UI.Text _txt_btn;
    [SerializeField] UnityEngine.UI.Text _txt_debug;
    [SerializeField] TMPro.TMP_InputField _if_obj;
    [SerializeField] TMPro.TMP_InputField _if_mtl;
    [SerializeField] TMPro.TMP_Text _txt_obj;
    [SerializeField] TMPro.TMP_Text _txt_mtl;

    [SerializeField] GameObject Spawn;

    GameObject ringMenus;
    RingBouton rb_previsous = null;
    Color[] colors;
    int btn_index_onthisRing;
    int btn_index;
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
            if (!dico.ContainsKey(hit.transform.name))
                Debug.Log(hit.transform.name);
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
                {
                    rb._SetSelectedColor();
                    GameObject selectedRingMenu = hit.transform.parent.parent.gameObject;

                    try
                    {
#if UNITY_WEBGL
                        MeshExporter.ObjExporter.OBJ_MTL_TXT om = MeshExporter.ObjExporter.MeshToString(selectedRingMenu, selectedRingMenu.name);
                        //display in 2 textboxes
                        _if_obj.text = om.obj;
                        _if_mtl.text = om.mtl;

                        _txt_obj.text = om.objfilename;
                        _txt_mtl.text = om.mtlfilename;

                        //save to disk // download file ??
                        MeshExporter.ObjExporter.OBJ_MTL_TXT.ToFile(om, @"E:\testjj");
#else
                        MeshExporter.ObjExporter.MeshToFile(selectedRingMenu, @"E:\");
#endif
                        _txt_debug.text = "";
                    }
                    catch (Exception ex)
                    {
                        _txt_debug.text = ex.Message + "\n\n" + ex.StackTrace;
                    }

                }
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
        Destroy(ringMenus);

        int nbr_buttons = (int)_sld_btns.value;
        Dictionary<int, List<List<int>>> operations = arc.Operation.OperationsGenerator(nbr_buttons, nbr_buttons, nbr_buttons);
        if (operations.ContainsKey(nbr_buttons))
            _txt_btns.text = $"{nbr_buttons} boutons [{operations[nbr_buttons].Count}]";
        else
            _txt_btns.text = $"{nbr_buttons} boutons [0]";

        ringMenus = COMPUTE(operations);
        ringMenus.transform.position = Spawn.transform.position;
        ringMenus.transform.rotation = Spawn.transform.rotation;
        ringMenus.transform.localScale = Spawn.transform.localScale;
    }

    private GameObject COMPUTE(Dictionary<int, List<List<int>>> operations)
    {
        dico = new Dictionary<string, RingBouton>();

        GameObject groups = new GameObject("Groups");
        foreach (int x in operations.Keys)
        {
            GameObject group = new GameObject("Btn " + x);
            int version = 0;

            float rayon_ext_prec = 0;
            foreach (List<int> entiers in operations[x])
            {
                GameObject ringMenu = new GameObject($"RingMenu {x}btn v{version}");

                float marge = _sld_marge.value;
                float epaisseur = 100;
                float rayon_ext = 0;
                int ringindex = 0;
                btn_index = 0;

                foreach (int nbrbuttons_by_ring in entiers)
                {
                    try
                    {
                        rayon_ext += epaisseur;
                        GameObject ring = DrawRing(ringindex, rayon_ext, epaisseur, nbrbuttons_by_ring, marge, group.name + " " + ringMenu.name + " ");
                        ring.transform.parent = ringMenu.transform;
                        _txt_debug.text = "";
                        ringindex++;
                    }
                    catch (Exception ex)
                    {
                        _txt_debug.text = ex.Message + "\n\n" + ex.StackTrace;
                    }
                }
                version++;

                ringMenu.transform.parent = group.transform;

                rayon_ext_prec = rayon_ext * 2 + rayon_ext_prec;
                ringMenu.transform.Translate(rayon_ext_prec, 0, 0);

                //optimisation sur 2 paramètres :
                //- p position sur l'axe centre du cercle, barycentre (position comprise entre rayon max et rayon min)
                //- t taille de l'icone
                //La meilleure solution p aura t maximum

                // first values
                // p0 = barycentre (parfait ou trop prêt du centre)
                // t0 = 1
                // test : est ce qu'aucun pixels du carré n'est vide (on a une image vide avec "seulement" le bouton de dessiné)
                // ==> on augmente t
                // quand tmax atteint pour p donné, on change de p, avec p compris entre p0 et p sur axe avec D = rayon_ext - t

                // puis dichtomie pour p en maximisant t à chaque essai.

            }


            group.transform.parent = groups.transform;
        }
        return groups;
    }

    GameObject DrawRing(int ring_index, float r_ext, float epaisseur, int nbrboutons, float marge, string prename)
    {
        btn_index_onthisRing = 0;
        GameObject go = new GameObject();
        float r_int = r_ext - epaisseur;

        float angle_ouverture_deg = (float)360 / nbrboutons;
        float angle_position_deg_init = angle_ouverture_deg / 2;

        for (int i = 0; i < nbrboutons; i++)
        {
            float angle_position_deg = angle_position_deg_init + i * angle_ouverture_deg;
            //bouton
            GameObject btn = DrawButton(r_ext,
                                r_int,
                                angle_ouverture_deg,
                                angle_position_deg,
                                marge);
            btn.name = prename + "ring_" + ring_index + "_btn_" + i;
            btn.transform.parent = go.transform;

            int index = btn_index;
            while (index >= textures.Length)
                index -= textures.Length;

            //icône
            Texture texture = (Texture)textures[index];
            GameObject icn = DrawIcon(btn, texture);
            if (icn != null)
                icn.transform.parent = go.transform;

            //script de gestion du bouton (index, nom, couleurs, ...) 
            RingBouton rb = btn.AddComponent<RingBouton>();
            rb._name = btn.name;
            rb._ring_index = ring_index;
            rb._index = i;
            rb._SetColors(colors[index]);
            rb._icone = icn;

            dico.Add(btn.name, rb);

            btn_index_onthisRing++;
            btn_index++;
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
        if (rend == null) return null;

        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;
        radius *= Mathf.Pow(0.5f, 0.5f);

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