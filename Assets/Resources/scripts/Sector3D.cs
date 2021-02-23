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
            Material mat = new Material(Shader.Find("Specular"));
            //Material mat = new Material(Shader.Find("Unlit/TransparentColored"));
            renderer.sharedMaterial = mat;
        }
        obj.name = name;
        return obj;
    }

    private static Mesh CreateMesh(float rayon_int, float rayon_ext, float angle_debut_deg, float angle_fin_deg, float marge, int nbrsegments)
    {
        if (nbrsegments < 1) nbrsegments = 1;
        float E = 0.1f; //Epaisseur
        float M = marge / nbrsegments;

        float Ri = rayon_int;
        float Re = rayon_ext - M / 4;
        float a_0 = angle_debut_deg;
        float a_1 = angle_fin_deg;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int> triangles = new List<int>();

        float Ox = (M) * Mathf.Sin((a_0 + a_1) / 2 / 180 * Mathf.PI);
        float Oy = (M) * Mathf.Cos((a_0 + a_1) / 2 / 180 * Mathf.PI);

        float Ax = Ox + (Ri) * Mathf.Sin(a_0 / 180 * Mathf.PI);
        float Ay = Oy + (Ri) * Mathf.Cos(a_0 / 180 * Mathf.PI);
        float Bx = Ox + (Re) * Mathf.Sin(a_0 / 180 * Mathf.PI);
        float By = Oy + (Re) * Mathf.Cos(a_0 / 180 * Mathf.PI);
        Vector3 a = new Vector3(Ax, 0, Ay);
        Vector3 b = new Vector3(Bx, 0, By);
        vertices.Add(a);
        vertices.Add(b);
        uv.Add(Vector3.forward);
        uv.Add(Vector3.forward);

        int it = 0; //indextriangles
        for (int i = 1; i < nbrsegments + 1; i++)
        {
            float a_01 = a_0 + (a_1 - a_0) * i / nbrsegments;

            Ax = Ox + (Ri) * Mathf.Sin(a_01 / 180 * Mathf.PI);
            Ay = Oy + (Ri) * Mathf.Cos(a_01 / 180 * Mathf.PI);
            Bx = Ox + (Re) * Mathf.Sin(a_01 / 180 * Mathf.PI);
            By = Oy + (Re) * Mathf.Cos(a_01 / 180 * Mathf.PI);

            a = new Vector3(Ax, 0, Ay);
            b = new Vector3(Bx, 0, By);
            vertices.Add(a);
            vertices.Add(b);

            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);

            triangles.AddRange(new int[] { it, it+1, it+2, //a, b, c
                                          it+1, it+3, it+2, //d, c, b
                                        });
            it += 2;
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
