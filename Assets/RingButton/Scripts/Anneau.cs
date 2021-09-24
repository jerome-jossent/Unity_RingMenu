using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Anneau
{
    public Dictionary<float, RingButton_EditorMode> butons_on_ring = new Dictionary<float, RingButton_EditorMode>();
    public Dictionary<int, RingButton_EditorMode> butons_on_ring_sorted;
    public int index;

    public float r_int;
    public float r_ext;
    public float marge;

    public Anneau(int index, List<float> rayons, float marge)
    {
        this.index = index;

        //0 ou celui d'avant
        if (index == 0)
            r_int = 0;
        else
            r_int = rayons[index - 1];
        
        r_ext = rayons[index];

        this.marge = marge;
    }

    public static List<float> GetRayons(List<float> epaisseurs)
    {
        List<float> rayons = new List<float>();
        float r_max_courant = 0;
        for (int i = 0; i < epaisseurs.Count; i++)
        {
            r_max_courant += epaisseurs[i];
            rayons.Add(r_max_courant);
        }
        return rayons;
    }

    public void Sort()
    {
        butons_on_ring_sorted = new Dictionary<int, RingButton_EditorMode>();

        var sorted = butons_on_ring.OrderBy(key => key.Key);
        for (int i = 0; i < sorted.Count(); i++)
        {
            var item = sorted.ElementAt(i);
            butons_on_ring_sorted.Add(i, item.Value);
            item.Value.button_index_on_ring_int = i;
        }
    }
}

