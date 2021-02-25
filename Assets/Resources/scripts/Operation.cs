using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arc
{
    public class Operation
    {
        public static Dictionary<int, List<List<int>>> OperationsGenerator(int sommeMin, int sommeMax, int valUnitaireMax)
        {
            //génère toutes les sommes possibles (uniques, valeurs croissantes)
            Dictionary<string, List<int>> val = new Dictionary<string, List<int>>();
            for (int i = 0; i < valUnitaireMax + 1; i++)
                for (int j = 0; j < valUnitaireMax + 1; j++)
                    for (int k = 0; k < valUnitaireMax + 1; k++)
                        for (int l = 0; l < valUnitaireMax + 1; l++)
                            if ((i == 0 || i < j) && (j == 0 || j < k) && (k == 0 || k < l))
                            //if (i <= j && j <= k && k <= l)
                            {
                                string clef = "";
                                if (i > 0) clef += $"{i},";
                                if (j > 0) clef += $"{j},";
                                if (k > 0) clef += $"{k},";
                                if (l > 0) clef += $"{l}";

                                if (i + j + k + l >= sommeMin && i + j + k + l <= sommeMax)
                                {
                                    List<int> entiers = new List<int>();

                                    if (i > 0) entiers = new List<int>() { i, j, k, l };
                                    else if (j > 0) entiers = new List<int>() { j, k, l };
                                    else if (k > 0) entiers = new List<int>() { k, l };
                                    else if (l > 0) entiers = new List<int>() { l };

                                    if (!val.ContainsKey(clef))
                                    {
                                        // 1 seul fois 1
                                        int nbr1 = 0;
                                        for (int ent = 0; ent < entiers.Count; ent++)
                                        {
                                            if (entiers[ent] == 1)
                                                nbr1++;
                                        }

                                        if (nbr1 < 2)
                                            val.Add(clef, entiers);
                                    }
                                }
                            }

            //rangement par somme
            Dictionary<int, List<List<int>>> operations = new Dictionary<int, List<List<int>>>();
            foreach (List<int> item in val.Values)
            {
                int somme = item.Sum();
                if (!operations.ContainsKey(somme))
                    operations.Add(somme, new List<List<int>>());
                operations[somme].Add(item);
            }
            return operations;
        }

    }
}
