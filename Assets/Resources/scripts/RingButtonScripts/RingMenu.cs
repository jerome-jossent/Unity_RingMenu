using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu
{
    public static GameObject _DrawRingMenu(List<int> nbrboutons,
                                    List<float> epaisseur,
                                    float marge,
                                    List<Color[]> couleurs,
                                    List<Texture[]> textures)
    {
        GameObject rm = new GameObject();
        float rayon = 0;
        for (int i = 0; i < nbrboutons.Count; i++)
        {
            rayon += epaisseur[i];
            GameObject a = Ring.DrawRing(i,
                rayon,
                epaisseur[i],
                nbrboutons[i],
                marge,
                couleurs[i],
                (textures != null) ? (textures.Count > i) ? textures[i] : null : null
                );
            a.transform.parent = rm.transform;
        }

        RingMenu_Manager rmm = rm.AddComponent<RingMenu_Manager>();
        rmm._ListAllButtons();
        return rm;
    }
}