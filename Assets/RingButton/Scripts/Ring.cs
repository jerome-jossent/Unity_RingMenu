using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring
{
    public static GameObject DrawRing(int ring_index,
                                      float r_ext,
                                      float epaisseur,
                                      int nbrboutons,
                                      float marge,
                                      Color[] couleurs,
                                      Texture[] textures)
    {
        GameObject go = new GameObject();
        go.name = "ring_" + ring_index;

        float r_int = r_ext - epaisseur;
        float angle_ouverture_deg = (float)360 / nbrboutons;
        float angle_position_deg_init = angle_ouverture_deg / 2;

        for (int i = 0; i < nbrboutons; i++)
        {
            float angle_position_deg = angle_position_deg_init + i * angle_ouverture_deg;

            GameObject btn;
            try
            {
                btn = RingButton.DrawButton(r_ext,
                                            r_int,
                                            angle_ouverture_deg,
                                            angle_position_deg,
                                            marge);
                if (btn == null) continue;

                btn.name = go.name + "_btn_" + i;
                btn.transform.parent = go.transform;

                RingButton_Manager rb = btn.AddComponent<RingButton_Manager>();
                rb._name = btn.name;
                rb._ring_index = ring_index;
                rb._index = i;
                rb._SetColors(couleurs[i]);

                //icône
                if (textures != null)
                {
                    try
                    {
                        Texture texture = textures[i];
                        GameObject icn = RingButton.DrawIcon(btn, texture);
                        if (icn != null)
                            icn.transform.parent = go.transform;
                        rb._icone = icn;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.Message + "\n" + ex.StackTrace);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message + "\n" + ex.StackTrace);
            }
        }
        return go;
    }

    public static GameObject DrawRing(int ring_index,
                                      float r_ext,
                                      float epaisseur,
                                      Dictionary<int, Bouton> boutons,
                                      float marge,
                                      Color[] couleurs,
                                      Texture[] textures)
    {
        GameObject go = new GameObject();
        go.name = "ring_" + ring_index;

        int nbrboutons = boutons.Count;

        float r_int = r_ext - epaisseur;
        float angle_ouverture_deg = (float)360 / nbrboutons;
        float angle_position_deg_init = angle_ouverture_deg / 2;

        foreach (Bouton bouton in boutons.Values)
        {
            float angle_position_deg = angle_position_deg_init + bouton.index * angle_ouverture_deg;

            GameObject btn;
            try
            {
                btn = RingButton.DrawButton(r_ext,
                                            r_int,
                                            angle_ouverture_deg,
                                            angle_position_deg,
                                            marge);
                if (btn == null) continue;

                btn.name = go.name + "_btn_" + bouton.label;
                btn.transform.parent = go.transform;

                RingButton_Manager rb = btn.AddComponent<RingButton_Manager>();
                rb._name = btn.name;
                rb._ring_index = ring_index;
                rb._index = bouton.index;
                rb._SetColors(couleurs[bouton.index]);

                //icône
                if (textures != null)
                {
                    try
                    {
                        Texture texture = textures[bouton.index];
                        GameObject icn = RingButton.DrawIcon(btn, texture);
                        if (icn != null)
                            icn.transform.SetParent(go.transform);
                        rb._icone = icn;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.Message + "\n" + ex.StackTrace);
                    }
                }

                //texte
                if (bouton.label != "")
                {
                    GameObject canvas_go = new GameObject();
                    canvas_go.transform.SetParent(rb.gameObject.transform);
                    Canvas canvas = canvas_go.gameObject.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.WorldSpace;
                    canvas.worldCamera = Camera.main;
                    float amplitude = r_int + r_ext * 0.8f;
                    canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(amplitude, amplitude);

                    GameObject text_go = new GameObject();
                    text_go.transform.SetParent(canvas_go.transform);
                    text_go.transform.Translate(0, 0, -20);
                    text_go.transform.Rotate(0, 0, angle_position_deg + angle_ouverture_deg / 2);

                    UnityEngine.UI.Text text = text_go.gameObject.AddComponent<UnityEngine.UI.Text>();
                    text.GetComponent<RectTransform>().sizeDelta = new Vector2(r_ext + r_int, r_ext + r_int);
                    text.alignment = TextAnchor.UpperCenter;
                    text.text = bouton.label;

                    text.font = bouton.label_font;
                    text.fontStyle = bouton.label_fontStyle;
                    text.resizeTextForBestFit = bouton.label_resizeTextForBestFit;
                    if (!bouton.label_resizeTextForBestFit)
                        text.fontSize = bouton.label_fontSize;
                    text.color = bouton.label_color;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message + "\n" + ex.StackTrace);
            }
        }
        return go;
    }
}