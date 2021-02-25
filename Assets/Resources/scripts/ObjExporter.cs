using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace MeshExporter
{
    //http://wiki.unity3d.com/index.php/ObjExporter

    //(https://stackoverflow.com/questions/46733430/convert-mesh-to-stl-obj-fbx-in-runtime)
    public static class ObjExporter
    {
        public static void MeshToFile(GameObject go, string filename)
        {
            Dictionary<string, string> MTLS = new Dictionary<string, string>();
            filename = filename.Replace(" ", "_");

            FileInfo fi = new FileInfo(filename);

            int indexLastTriangle = 0;
            string chaineOBJ = "#JJO\nmtllib " + fi.Name + ".mtl\n\n";
            MeshToString(go, MTLS, ref chaineOBJ, ref indexLastTriangle);

            StringBuilder sb = new StringBuilder();
            foreach (string clef in MTLS.Keys)
            {
                sb.Append("newmtl " + clef).Append("\n");
                sb.Append(MTLS[clef]).Append("\n");
                sb.Append("\n");                
            }
            using (StreamWriter sw = new StreamWriter(filename + ".mtl"))
                sw.Write(sb.ToString());

            using (StreamWriter sw = new StreamWriter(filename + ".obj"))
                sw.Write(chaineOBJ);

        }

        public static void MeshToString(GameObject go, Dictionary<string,string> MTLS, ref string chaineOBJ, ref int indexLastTriangle)
        {
            int jj = indexLastTriangle;
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null)
            {
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

                int max = jj;
                for (int material = 0; material < m.subMeshCount; material++)
                {
                    sb.Append("\n");
                    sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                    sb.Append("usemap ").Append(mats[material].name).Append("\n");
                    if (!MTLS.ContainsKey(mats[material].name))
                    {
                        Material mat = mats[material];
                        MTLS.Add(mats[material].name, string.Format($"Kd {mat.color.r} {mat.color.g} {mat.color.b} {mat.color.a}").Replace(",", "."));
                    }

                    int[] t = m.GetTriangles(material);
                    for (int i = 0; i < t.Length; i += 3)
                    {
                        sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", t[i] + 1 + jj, t[i + 1] + 1 + jj, t[i + 2] + 1 + jj));
                        max = Mathf.Max(new int[] { max, t[i] + 1 + jj, t[i + 1] + 1 + jj, t[i + 2] + 1 + jj });
                    }
                    jj = max;
                }
                sb.Append("\n");

                indexLastTriangle = jj;
                chaineOBJ += sb.ToString();
            }

            for (int i = 0; i < go.transform.childCount; i++)            
                MeshToString(go.transform.GetChild(i).gameObject, MTLS, ref chaineOBJ, ref indexLastTriangle);            

        }

        //public static string MeshToString(GameObject go)
        //{
        //    MeshFilter mf = go.GetComponent<MeshFilter>();
        //    Mesh m = mf.mesh;
        //    Renderer rd = go.GetComponent<Renderer>();
        //    Material[] mats = rd.sharedMaterials;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("g ").Append(mf.name).Append("\n");

        //    foreach (Vector3 v in m.vertices)
        //        sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z).Replace(",", "."));

        //    sb.Append("\n");
        //    foreach (Vector3 v in m.normals)
        //        sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z).Replace(",", "."));

        //    sb.Append("\n");
        //    foreach (Vector3 v in m.uv)
        //        sb.Append(string.Format("vt {0} {1}\n", v.x, v.y).Replace(",", "."));

        //    for (int material = 0; material < m.subMeshCount; material++)
        //    {
        //        sb.Append("\n");
        //        sb.Append("usemtl ").Append(mats[material].name).Append("\n");
        //        sb.Append("usemap ").Append(mats[material].name).Append("\n");

        //        int[] t = m.GetTriangles(material);
        //        for (int i = 0; i < t.Length; i += 3)
        //            sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", t[i] + 1, t[i + 1] + 1, t[i + 2] + 1));
        //    }
        //    return sb.ToString();
        //}


        public static void CombineMeshes(GameObject parent)
        {
            MeshFilter targetFilter = null;

            targetFilter = parent.GetComponent<MeshFilter>();

            if (targetFilter == null)
            {
                targetFilter = parent.AddComponent<MeshFilter>();
            }

            MeshRenderer targetRenderer = null;

            targetRenderer = parent.GetComponent<MeshRenderer>();

            if (targetRenderer == null)
            {
                targetRenderer = parent.AddComponent<MeshRenderer>();
            }

            MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int index = 0;

            int matIndex = -1;

            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].sharedMesh == null) continue;
                if (meshFilters[i].gameObject.GetComponent<Renderer>().enabled == false)
                {
                    continue;
                }
                else if (matIndex == -1)
                {
                    matIndex = i;
                }
                if (meshFilters[i].Equals(targetFilter)) continue;


                combine[index].mesh = meshFilters[i].sharedMesh;

                combine[index++].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.GetComponent<Renderer>().enabled = false;
            }

            targetFilter.mesh.CombineMeshes(combine);

            targetFilter.gameObject.GetComponent<Renderer>().material = meshFilters[matIndex].gameObject.GetComponent<Renderer>().material;

        }


    }
}