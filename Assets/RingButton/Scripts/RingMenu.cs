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
            GameObject ring = null;
            try
            {
                rayon += epaisseur[i];
                ring = Ring.DrawRing(i,
                    rayon,
                    epaisseur[i],
                    nbrboutons[i],
                    marge,
                    couleurs[i],
                    (textures != null) ? (textures.Count > i) ? textures[i] : null : null
                    );
                ring.transform.parent = rm.transform;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message + "\n" + ex.StackTrace);
            }                
        }

        RingMenu_Manager rmm = rm.AddComponent<RingMenu_Manager>();
        rmm._ListAllButtons();

        return rm;
    }

    public static GameObject _DrawRingMenu(List<Dictionary<int, Bouton>> boutons,
                                    List<float> epaisseur,
                                    float marge,
                                    List<Color[]> couleurs)
    {
        GameObject rm = new GameObject();
        float rayon = 0;
        for (int i = 0; i < boutons.Count; i++)
        {
            GameObject ring = null;
            try
            {
                rayon += epaisseur[i];
                ring = Ring.DrawRing(i,
                    rayon,
                    epaisseur[i],
                    boutons[i],
                    marge,
                    couleurs[i]
                    //,
                    //(textures != null) ? (textures.Count > i) ? textures[i] : null : null
                    );
                ring.transform.parent = rm.transform;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message + "\n" + ex.StackTrace);
            }
        }

        RingMenu_Manager rmm = rm.AddComponent<RingMenu_Manager>();
        rmm._ListAllButtons();

        return rm;
    }
}