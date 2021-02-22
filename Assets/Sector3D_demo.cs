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

    [SerializeField] UnityEngine.UI.Text _txt;

    [SerializeField] GameObject Spawn;
    GameObject boutons;

    Color[] colors;
    int btn_index;

    private void Start()
    {
        colors = new Color[100];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 0.8f, 0.8f);


        _SliderValueChange();
    }

    public void _SliderValueChange()
    {
        Destroy(boutons);
        btn_index = 0;
        boutons = new GameObject();

        int R0_B = (int)_sld_anneau0_btn.value;
        int R1_B = (int)_sld_anneau1_btn.value;
        int R2_B = (int)_sld_anneau2_btn.value;
        int R3_B = (int)_sld_anneau3_btn.value;
        int R4_B = (int)_sld_anneau4_btn.value;

        float R0_R = _sld_anneau0_taille.value;
        float R1_R = _sld_anneau1_taille.value;
        float R2_R = _sld_anneau2_taille.value;
        float R3_R = _sld_anneau3_taille.value;
        float R4_R = _sld_anneau4_taille.value;

        int marge = 0;
        float r_max = R0_R + R1_R + R2_R + R3_R + R4_R;
        //dico = new Dictionary<string, Button>();

        GameObject a0 = DrawSecteurs(0, R0_R, r_max, R0_R, R0_B, marge);
        GameObject a1 = DrawSecteurs(1, R0_R + R1_R, r_max, R1_R, R1_B, marge);
        GameObject a2 = DrawSecteurs(2, R0_R + R1_R + R2_R, r_max, R2_R, R2_B, marge);
        GameObject a3 = DrawSecteurs(3, R0_R + R1_R + R2_R + R3_R, r_max, R3_R, R3_B, marge);
        GameObject a4 = DrawSecteurs(4, R0_R + R1_R + R2_R + R3_R + R4_R, r_max, R4_R, R4_B, marge);

        _txt.text = btn_index + " boutons";

        a0.transform.parent = boutons.transform;
        a1.transform.parent = boutons.transform;
        a2.transform.parent = boutons.transform;
        a3.transform.parent = boutons.transform;
        a4.transform.parent = boutons.transform;

        boutons.transform.position = Spawn.transform.position;
        boutons.transform.rotation = Spawn.transform.rotation;
        boutons.transform.localScale = Spawn.transform.localScale;
    }

    GameObject DrawSecteurs(int ring_index, float r_ext, float r_extMAX, float epaisseur, int nbrboutons, float marge)
    {
        GameObject go = new GameObject();
        float r_int = r_ext - epaisseur;

        float angle_ouverture_deg = (float)360 / nbrboutons;
        float angle_position_deg_init = angle_ouverture_deg / 2;

        //Point c = new Point(r_ext, r_ext);
        //Grid g = new Grid();
        //g.Width = r_ext * 2;
        //g.Height = r_ext * 2;
        for (int i = 0; i < nbrboutons; i++)
        {
            //Button b = new Button();
            float angle_position_deg = angle_position_deg_init + i * angle_ouverture_deg;
            GameObject p = DrawSecteur(r_ext,
                                r_extMAX,
                                r_int,
                                marge,
                                angle_ouverture_deg,
                                angle_position_deg);

            p.name = "ring_" + ring_index + "_btn_" + i;
            p.transform.parent = go.transform;
            //int j = i;
            //while (j >= colors.Count)
            //    j -= colors.Count;
            //p.Fill = colors[j];

            //b.couleur = p.Fill;
            //Color color = ((SolidColorBrush)b.couleur).Color;
            //float fctr = 0.8f;
            //byte offest = 50;
            //b.couleurfonce = new SolidColorBrush(Color.FromRgb(ChangeIntensity(color.R, fctr, offest),
            //                                                    ChangeIntensity(color.G, fctr, offest),
            //                                                    ChangeIntensity(color.B, fctr, offest)));
            //b.btn_index = i;
            //b.name = p.Name;

            //dico.Add(b.name, b);
            //g.Children.Add(p);
        }

        //Viewbox v = new Viewbox();
        //v.StretchDirection = StretchDirection.Both;
        //v.Stretch = Stretch.UniformToFill;
        //v.Width = r_ext;
        //v.Height = r_ext;
        //v.Child = g;
        //v.Margin = new Thickness((r_extMAX - r_ext) / 2);
        //return v;

        return go;
    }

    private GameObject DrawSecteur(float r_ext, float r_extMAX, float r_int, float marge, float angle_ouverture_deg, float angle_position_deg)
    {
        GameObject secteur1 = Sector3D.CreateObject(r_int, r_ext, angle_position_deg, angle_position_deg + angle_ouverture_deg, name: "A0");

        //MaterialSetColor.Colorier(secteur1, new Color(1f, 0f, 0f, 0.5f));
        MaterialSetColor.Colorier(secteur1, colors[btn_index]);
        btn_index++;
        secteur1.AddComponent<ClickOnCollider>();

        return secteur1;
    }
}
