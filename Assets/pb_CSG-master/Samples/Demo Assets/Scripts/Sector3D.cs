using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector3D : MonoBehaviour
{
    public static GameObject CreateObject(float rayon_int, float rayon_ext, float angle_debut_deg, float angle_fin_deg, int? nbrsegments = null, string name ="Sector3D")
    {
        //j'ai estimé qu'une "courbure" ne se voyait plus en dessous de 5°
        if (nbrsegments == null)
            nbrsegments = Mathf.CeilToInt((angle_fin_deg - angle_debut_deg) / 5);

        var obj = new GameObject("Sector3D");
        var mesh = CreateMesh(rayon_int, rayon_ext, angle_debut_deg, angle_fin_deg, (int)nbrsegments);
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        renderer.sharedMaterial = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
        obj.name = name;
        return obj;
    }

    private static Mesh CreateMesh(float rayon_int, float rayon_ext, float angle_debut_deg, float angle_fin_deg, int nbrsegments)
    {
        if (nbrsegments < 1) nbrsegments = 1;
        float Ri = rayon_int;
        float Re = rayon_ext;
        float a_0 = angle_debut_deg;
        float a_1 = angle_fin_deg;

        float E = 0.1f; //Epaisseur

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<int> triangles = new List<int>();

        float Ax = Ri * Mathf.Sin(a_0 / 180 * Mathf.PI);
        float Ay = Ri * Mathf.Cos(a_0 / 180 * Mathf.PI);
        float Bx = Re * Mathf.Sin(a_0 / 180 * Mathf.PI);
        float By = Re * Mathf.Cos(a_0 / 180 * Mathf.PI);
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

            Ax = Ri * Mathf.Sin(a_01 / 180 * Mathf.PI);
            Ay = Ri * Mathf.Cos(a_01 / 180 * Mathf.PI);
            Bx = Re * Mathf.Sin(a_01 / 180 * Mathf.PI);
            By = Re * Mathf.Cos(a_01 / 180 * Mathf.PI);

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
