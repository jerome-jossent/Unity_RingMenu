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
}