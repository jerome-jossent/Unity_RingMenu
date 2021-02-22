using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace MeshExporter
{
    //http://wiki.unity3d.com/index.php/ObjExporter

    //(https://stackoverflow.com/questions/46733430/convert-mesh-to-stl-obj-fbx-in-runtime)
    public static class ObjExporter
    {
        public static void MeshToFile(GameObject go, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))            
                sw.Write(MeshToString(go));            
        }

        public static string MeshToString(GameObject go)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            Mesh m = mf.mesh;
            Renderer rd = go.GetComponent<Renderer>();
            Material[] mats = rd.sharedMaterials;

            StringBuilder sb = new StringBuilder();
            sb.Append("g ").Append(mf.name).Append("\n");

            foreach (Vector3 v in m.vertices)
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z).Replace(",", "."));

            sb.Append("\n");
            foreach (Vector3 v in m.normals)
                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z).Replace(",", "."));

            sb.Append("\n");
            foreach (Vector3 v in m.uv)
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y).Replace(",", "."));

            for (int material = 0; material < m.subMeshCount; material++)
            {
                sb.Append("\n");
                sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                sb.Append("usemap ").Append(mats[material].name).Append("\n");

                int[] t = m.GetTriangles(material);
                for (int i = 0; i < t.Length; i += 3)
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", t[i] + 1, t[i + 1] + 1, t[i + 2] + 1));                
            }
            return sb.ToString();
        }

    }
}