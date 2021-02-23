#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

public class Triangle3DPrimitive
{
#if UNITY_EDITOR
    private static Mesh CreateMesh()
    {
        float R = 1f;
        float angle_deg = 45f;

        float X = R * Mathf.Sin(angle_deg / 2);
        float X_ = -X;
        float Z = R * Mathf.Cos(angle_deg / 2);

        float E = 0.1f; //Epaisseur

        Vector3 a = new Vector3(0, 0, 0);
        Vector3 b = new Vector3(X_, 0, Z);
        Vector3 c = new Vector3(X, 0, Z);
        Vector3 d = new Vector3(0, E, 0);
        Vector3 e = new Vector3(X_, E, Z);
        Vector3 f = new Vector3(X, E, Z);

        Vector3[] vertices = {a,b,c,d,e,f,};

        Vector2[] uv = {
             Vector3.forward,
             Vector3.forward,
             Vector3.forward,
             Vector3.forward,
             Vector3.forward,
             Vector3.forward,
         };


        int[] triangles = {

            2, 1, 0, //c,b,a
            3, 4, 5, //d,e,f
            0, 1, 3, //a,b,d
            3, 1, 4, //d, b, e
            2,0,5,//c,a,f
            5,0,3,//f,a,d
            2,5,1,//c,f,b
            5,4,1//f,e,b
        };

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    private static GameObject CreateObject()
    {
        var obj = new GameObject("Triangle3D");
        var mesh = CreateMesh();
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        return obj;
    }

    public static GameObject CreateTriangle()
    {
        return CreateObject();
    }

    [MenuItem("GameObject/3D Object/Triangle3D", false, 0)]
    public static void Create()
    {
        CreateObject();
    }
#endif
}