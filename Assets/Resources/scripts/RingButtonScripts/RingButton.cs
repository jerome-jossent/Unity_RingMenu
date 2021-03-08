using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingButton
{
    public static GameObject DrawButton(float r_ext, 
        float r_int, 
        float angle_ouverture_deg, 
        float angle_position_deg, 
        float marge)
    {
        return CreateSector3D(r_int, 
            r_ext, 
            angle_position_deg, 
            angle_position_deg + angle_ouverture_deg, 
            marge, 
            name: "A0");
    }

    public static GameObject CreateSector3D(float rayon_int, 
        float rayon_ext, 
        float angle_debut_deg, 
        float angle_fin_deg, 
        float marge, 
        int? nbrsegments = null, 
        string name = "Sector3D")
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

    static Mesh CreateMesh(float rayon_int, float rayon_ext,
                           float angle_debut_deg, float angle_fin_deg,
                           float marge, int nbrsegments)
    {
        #region infos
        //c'est la gestion de la marge qui rend les choses complexes
        //l'idée : le secteur (angle béta) a créer est centré sur l'axe Y
        //il sera pivoter plus tard
        //on créé les points du centre vers le sens horaire jusqu'à béta/2 = alpha
        //puis du centre vers le sens anti horaire idem jusqu'à - alpha
        #endregion

        #region variables
        nbrsegments /= 2;
        if (nbrsegments < 1) nbrsegments = 1;
        //float E = 0.1f; //Epaisseur
        //float M = marge / nbrsegments;
        //float Ri = rayon_int + M/2;
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

        Vector2 O = new Vector2(0, 0);
        Math_JJ.Cercle ce = new Math_JJ.Cercle() { O = O, r = Re };
        Math_JJ.Cercle ci = new Math_JJ.Cercle() { O = O, r = Ri };

        // origine prime (à cause de la marge) sur l'axe Y (donc O'x=0)
        float Opy = M / Mathf.Sin((90 - alpha) / 180 * Mathf.PI);
        #endregion

        #region premiers points en partant du milieu (sur l'axe Y)
        Vector2 A = new Vector2(0, Ri);
        Vector2 B = new Vector2(0, Re);
        vertices.Add(A);
        vertices.Add(B);
        uv.Add(Vector3.forward);
        uv.Add(Vector3.forward);
        #endregion

        #region points => horaire
        int it = 0; //indextriangles
        for (int i = 1; i < nbrsegments + 1; i++)
        {
            float a_01 = alpha * i / nbrsegments;
            float a1 = Mathf.Tan((90 - a_01) / 180 * Mathf.PI);
            Math_JJ.Droite d = new Math_JJ.Droite() { a = a1, b = Opy };
            A = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, a_01);
            B = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, a_01);
            vertices.Add(A);
            vertices.Add(B);
            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);
            triangles.AddRange(new int[] { it,   it+1, it+2, //a, b, c
                                           it+1, it+3, it+2  //b, d, c
                                         });
            it += 2;
        }
        #endregion

        #region (encore) premiers points en partant du milieu (sur l'axe Y)
        A = new Vector2(0, Ri);
        B = new Vector2(0, Re);
        vertices.Add(A);
        vertices.Add(B);
        uv.Add(Vector3.forward);
        uv.Add(Vector3.forward);
        #endregion

        #region points => anti-horaire
        it += 2;
        for (int i = 1; i < nbrsegments + 1; i++)
        {
            float a_01 = alpha * i / nbrsegments;
            float a1 = Mathf.Tan((90 - a_01) / 180 * Mathf.PI);
            Math_JJ.Droite d = new Math_JJ.Droite() { a = a1, b = Opy };
            A = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, a_01);
            B = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, a_01);
            vertices.Add(new Vector3(-A.x, A.y));
            vertices.Add(new Vector3(-B.x, B.y));
            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);
            triangles.AddRange(new int[] { it,   it+2, it+1, //a, c, b
                                           it+1, it+2, it+3  //b, c, d
                                         });
            it += 2;
        }
        #endregion

        #region rotation de tous les points (= du secteur) à la position demandée
        for (int i = 0; i < vertices.Count; i++)
            vertices[i] = Quaternion.Euler(0, 0, rotation) * vertices[i];
        #endregion

        #region création du MESH
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
        #endregion
    }

    public static GameObject DrawIcon(GameObject secteur, Texture icone)
    {
        // A sphere that fully encloses the bounding box.
        //https://docs.unity3d.com/ScriptReference/Renderer-bounds.html
        Renderer rend = secteur.GetComponent<Renderer>();
        if (rend == null) return null;

        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;
        radius *= Mathf.Pow(0.5f, 0.5f);

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = new Material(Shader.Find("Unlit/TransparentColored"));
        mat.mainTexture = icone;
        quad.GetComponent<Renderer>().material = mat;

        quad.transform.Rotate(90, 0, 0);
        quad.transform.localScale = new Vector3(radius, radius);
        quad.transform.position = center + Vector3.up;

        return quad;
    }
}
