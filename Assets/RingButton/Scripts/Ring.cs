using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace RingMenuJJ
{
    public class Ring
    {
        public static GameObject DrawRing(int ring_index,
                                          float r_ext,
                                          float epaisseur,
                                          int nbrboutons,
                                          float marge,
                                          Color[] couleurs,
                                          Texture[] textures
            )
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
                    //rb._name = btn.name;
                    rb._ring_index = ring_index;
                    rb._index = i;
                    rb._SetColors(couleurs[i]);

                    //icône
                    if (textures != null)
                    {
                        try
                        {
                            Texture texture = textures[i];
                            GameObject icn = RingButton.DrawIcon(texture, 0, 0, 0);
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
                                              Color[] couleurs
                )
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

                    btn.name = go.name + "_btn_" + bouton.name;
                    btn.transform.parent = go.transform;

                    RingButton_Manager rb = btn.AddComponent<RingButton_Manager>();
                    //rb._name = btn.name;
                    rb._ring_index = ring_index;
                    rb._index = bouton.index;
                    rb._SetColors(couleurs[bouton.index]);

                    //icône
                    if (bouton.icone != null)
                    {
                        try
                        {
                            Texture texture = bouton.icone;
                            GameObject icn = RingButton.DrawIcon(texture, 0, 0, 0);
                            if (icn != null)
                            {
                                icn.transform.parent = rb.gameObject.transform;
                                float hauteur = (r_ext - r_int - marge) / Mathf.Pow(2, 0.5f);

                                float amplitude = r_int + hauteur / 2 + marge;
                                float x = amplitude * Mathf.Cos((90 + angle_position_deg + angle_ouverture_deg / 2) / 180 * Mathf.PI);
                                float y = amplitude * Mathf.Sin((90 + angle_position_deg + angle_ouverture_deg / 2) / 180 * Mathf.PI);

                                if (angle_ouverture_deg == 360)
                                {
                                    x = 0;
                                    y = 0;
                                    hauteur = r_ext;
                                }
                                icn.transform.localScale = new Vector2(hauteur, hauteur);
                                icn.transform.Translate(x, y, -35);

                                //icn.transform.Rotate(0, 0, angle_position_deg + angle_ouverture_deg / 2);
                            }
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

        public static GameObject DrawRing(Anneau anneau,
                                          float angle_initial,
                                          bool sens_horaire)
        {
            float r_int = anneau.r_int;
            float r_ext = anneau.r_ext;
            float marge = anneau.marge * (r_ext - r_int);

            GameObject go = new GameObject();
            go.name = "ring_" + anneau.index;

            int nbrboutons = anneau.butons_on_ring.Count;

            float angle_ouverture_deg = (float)360 / nbrboutons;

            if (sens_horaire)
                angle_initial = -angle_initial;

            float angle_position_deg_init = angle_initial + angle_ouverture_deg / 2;

            foreach (RingButton_EditorMode bouton in anneau.butons_on_ring.Values)
            {
                float angle_position_deg;
                if (sens_horaire)
                    angle_position_deg = angle_position_deg_init - (bouton.button_index_on_ring_int + 1) * angle_ouverture_deg;
                else
                    angle_position_deg = angle_position_deg_init + (bouton.button_index_on_ring_int - 1) * angle_ouverture_deg;

                //Debug.Log("Bouton \"" + bouton.name + "\" angle " + (int)angle_position_deg + " sur " + (int)angle_ouverture_deg + "°");

                GameObject btn;
                try
                {
                    btn = RingButton.DrawButton(r_ext,
                                                r_int,
                                                angle_ouverture_deg,
                                                angle_position_deg,
                                                marge);
                    if (btn == null) continue;

                    btn.name = go.name + "_btn_" + bouton.name;
                    btn.transform.parent = go.transform;

                    RingButton_Manager rb = btn.AddComponent<RingButton_Manager>();
                    bouton.ringButtonManager = rb;
                    rb._ring_index = anneau.index;
                    rb._index = bouton.button_index_on_ring_int;
                    rb._SetColors(bouton.button_color);

                    //icône
                    if (bouton.icon != null)
                    {
                        try
                        {
                            Texture texture = bouton.icon;
                            GameObject icn = RingButton.DrawIcon(texture,
                                r_ext, r_int,
                                angle_position_deg + angle_ouverture_deg / 2);

                            icn.transform.parent = rb.gameObject.transform;
                            rb._icone = icn;
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message + "\n" + ex.StackTrace);
                        }
                    }

                    //texte
                    if (bouton.label.label_show && bouton.label.label != "")
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
                        text.text = bouton.label.label;

                        text.font = bouton.label.label_font;
                        text.fontStyle = bouton.label.label_fontStyle;
                        text.resizeTextForBestFit = bouton.label.label_resizeTextForBestFit;
                        if (!bouton.label.label_resizeTextForBestFit)
                            text.fontSize = bouton.label.label_fontSize;
                        text.color = bouton.label.label_color;
                    }

                    //events
                    rb._OnClick = bouton.events._OnClick;
                    rb._OnEnter = bouton.events._OnEnter;
                    rb._OnExit = bouton.events._OnExit;
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message + "\n" + ex.StackTrace);
                }
            }
            return go;
        }


    }
}