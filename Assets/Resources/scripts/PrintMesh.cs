using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintMesh : MonoBehaviour
{
    [SerializeField] MeshRenderer r;
    [SerializeField] Mesh m;
    [SerializeField] Vector3[] vertices;
    [SerializeField] Vector3[] normals;
    [SerializeField] Vector2[] uv;


    void Start()
    {
        r = gameObject.GetComponent<MeshRenderer>();
        m = gameObject.GetComponent<MeshFilter>().mesh;

        vertices = m.vertices;
        normals = m.normals;
        uv = m.uv;
    }
}
