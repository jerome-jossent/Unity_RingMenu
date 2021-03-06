using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector3D : MonoBehaviour
{
    public static GameObject CreateObject(float rayon_int, float rayon_ext, float angle_debut_deg, float angle_fin_deg, float marge, int? nbrsegments = null, string name = "Sector3D")
    {
        //j'ai estimé qu'une "courbure" ne se voyait plus en dessous de 5°
        if (nbrsegments == null)
            nbrsegments = Mathf.CeilToInt((angle_fin_deg - angle_debut_deg) / 5);

        var obj = new GameObject("Sector3D");
        if (rayon_ext > rayon_int)
        {
            var mesh = CreateMesh(rayon_int, rayon_ext, angle_debut_deg, angle_fin_deg, marge, (int)nbrsegments);
            var filter = obj.AddComponent<MeshFilter>();
            var renderer = obj.AddComponent<MeshRenderer>();
            var collider = obj.AddComponent<MeshCollider>();

            filter.sharedMesh = mesh;
            collider.sharedMesh = mesh;
            Material mat = new Material(Shader.Find("Unlit/TransparentColored"));
            renderer.sharedMaterial = mat;
        }
        obj.name = name;
        return obj;
    }

    static Vector3 FindPointOnCircleAndLine(float pente, float offset, float rayon, float angle)
    {
        // cercle centré : x² + y² = r² & droite coupant le cercle y = a*x + b
        // en passant par le centre du cercle on sait qu'on aura toujours 2 intersections = 2 points = 2 solutions
        // x² + (a²*x² + b² + 2*a*x*b) = r²      
        // (1+a²)*x² + 2*a*b*x + b²-r² = 0
        // si Ax²+Bx+C ==> delta = B²-4*A*C
        float delta_Bx = Mathf.Pow(2 * pente * offset, 2) - 4 * (1 + pente * pente) * (offset * offset - rayon * rayon);
        float Bx1 = (-(2 * pente * offset) - Mathf.Pow(delta_Bx, 0.5f)) / (2 * (1 + pente * pente));
        float Bx2 = (-(2 * pente * offset) + Mathf.Pow(delta_Bx, 0.5f)) / (2 * (1 + pente * pente));
        float By1, By2;

        if (Bx1 == 0)
        {
            By1 = rayon;
            By2 = -rayon;
        }
        else
        {
            By1 = pente * Bx1;
            By2 = pente * Bx2;
        }

        Vector2 B1 = new Vector2(Bx1, By1);
        Vector2 B2 = new Vector2(Bx2, By2);

        //Bx1 ou Bx2 ==> celui qui est le plus proche du point sans marge ??
        float Bx = rayon * Mathf.Sin(angle / 180 * Mathf.PI);
        float By = rayon * Mathf.Cos(angle / 180 * Mathf.PI);
        Vector3 b = new Vector3(Bx, 0, By);

        if (Vector2.Distance(b, B1) < Vector2.Distance(b, B2))
            b = B1;
        else
            b = B2;

        return b;
    }

    private static Mesh CreateMesh(float rayon_int, float rayon_ext, float angle_debut_deg, float angle_fin_deg, float marge, int nbrsegments)
    {
        nbrsegments /= 2;
        if (nbrsegments < 1) nbrsegments = 1;
        //float E = 0.1f; //Epaisseur

        //        float M = marge / nbrsegments;
        //        float Ri = rayon_int + M/2;
        float M = marge;
        float Ri = rayon_int + M;
        float Re = rayon_ext;
        float a_0 = angle_debut_deg;
        float a_1 = angle_fin_deg;

        float beta = a_1 - a_0;
        float alpha = beta / 2;
        float rotation = alpha + angle_debut_deg;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int> triangles = new List<int>();

        // premiers points en partant du milieu (sur l'axe Y)
        Vector2 A = new Vector2(0, Ri);
        Vector2 B = new Vector2(0, Re);
        vertices.Add(A);
        vertices.Add(B);
        uv.Add(Vector3.forward);
        uv.Add(Vector3.forward);

        Vector2 O = new Vector2(0, 0);

        //origine prime (à cause de la marge)
        float Opy = M / Mathf.Sin((90 - alpha) / 180 * Mathf.PI);
        Vector2 Op = new Vector2(0, Opy);
        float a1 = Mathf.Tan((90 - alpha) / 180 * Mathf.PI);

        // derniers points côté fin rotation horaire
        Math_JJ.Droite d = new Math_JJ.Droite() { a = a1, b = Opy };
        Math_JJ.Cercle ce = new Math_JJ.Cercle() { O = O, r = Re };
        Math_JJ.Cercle ci = new Math_JJ.Cercle() { O = O, r = Ri };
        Vector2 Y = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, alpha);
        Vector2 Z = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, alpha);

        int it = 0; //indextriangles
        for (int i = 1; i < nbrsegments + 1; i++)
        {
            float a_01 = alpha * i / nbrsegments;
            a1 = Mathf.Tan((90 - a_01) / 180 * Mathf.PI);
            d = new Math_JJ.Droite() { a = a1, b = Opy };
            A = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, a_01);
            B = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, a_01);

            vertices.Add(A);
            vertices.Add(B);

            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);

            triangles.AddRange(new int[] { it, it+1, it+2, //a, b, c
                                              it+1, it+3, it+2, //b, d, c
                                            });
            it += 2;
        }

        A = new Vector2(0, Ri);
        B = new Vector2(0, Re);
        vertices.Add(A);
        vertices.Add(B);
        uv.Add(Vector3.forward);
        uv.Add(Vector3.forward);
        it += 2;
        for (int i = 1; i < nbrsegments + 1; i++)
        {
            float a_01 = alpha * i / nbrsegments;
            a1 = Mathf.Tan((90 - a_01) / 180 * Mathf.PI);
            d = new Math_JJ.Droite() { a = a1, b = Opy };
            A = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, a_01);
            B = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, a_01);

            vertices.Add(new Vector3(-A.x, A.y));
            vertices.Add(new Vector3(-B.x, B.y));
            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);

            triangles.AddRange(new int[] { it, it+2, it+1, //a, c, b
                                              it+1, it+2, it+3, //b, c, d
                                            });
            it += 2;
        }

        //rotation de tous les points (= du secteur) à la position demandée
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = Quaternion.Euler(0, 0, rotation) * vertices[i];
        }

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }
}
