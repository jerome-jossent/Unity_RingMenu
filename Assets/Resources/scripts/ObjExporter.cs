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
        public class OBJ_MTL_TXT
        {
            public string obj, mtl;
            public string objfilename, mtlfilename;

            public static void ToFile(OBJ_MTL_TXT obj_mtl_txt, string directory = "")
            {
                if (directory != "")
                {
                    DirectoryInfo di = new DirectoryInfo(directory);
                    if (!di.Exists)
                        di.Create();

                    obj_mtl_txt.objfilename = di.FullName + "\\" + obj_mtl_txt.objfilename;
                    obj_mtl_txt.mtlfilename = di.FullName + "\\" + obj_mtl_txt.mtlfilename;                   
                }

                using (StreamWriter sw = new StreamWriter(obj_mtl_txt.mtlfilename))
                    sw.Write(obj_mtl_txt.mtl);
                using (StreamWriter sw = new StreamWriter(obj_mtl_txt.objfilename))
                    sw.Write(obj_mtl_txt.obj);
            }
        }

        public static void MeshToFile(GameObject go, string filename)
        {
            OBJ_MTL_TXT om = MeshToString(go, filename);
            OBJ_MTL_TXT.ToFile(om, filename);
        }


        public static OBJ_MTL_TXT MeshToString(GameObject go, string filename)
        {
            Dictionary<string, string> MTLS = new Dictionary<string, string>();
            filename = filename.Replace(" ", "_");

            FileInfo fi = new FileInfo(filename);
            string objFileName = fi.Name + ".obj";
            string mtlFileName = fi.Name + ".mtl";

            int indexLastTriangle = 0;
            string chaineOBJ = "#JJO\nmtllib " + mtlFileName + "\n\n";
            MeshToString(go, MTLS, ref chaineOBJ, ref indexLastTriangle);

            StringBuilder sb = new StringBuilder();
            sb.Append("#JJO\n");
            foreach (string clef in MTLS.Keys)
            {
                sb.Append("newmtl " + clef).Append("\n");
                sb.Append(MTLS[clef]).Append("\n");
                sb.Append("\n");
            }

            OBJ_MTL_TXT om = new OBJ_MTL_TXT()
            {
                mtl = sb.ToString(),
                obj = chaineOBJ,
                objfilename = objFileName,
                mtlfilename = mtlFileName
            };
            return om;
        }

        public static void MeshToString(GameObject go, Dictionary<string, string> MTLS, ref string chaineOBJ, ref int indexLastTriangle)
        {
            int jj = indexLastTriangle;
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null)
            {
                Mesh m = mf.mesh;
                Renderer rd = go.GetComponent<Renderer>();
                Material[] mats = rd.sharedMaterials;

                StringBuilder sb = new StringBuilder();
                sb.Append("g ").Append(mf.name.Replace(" ", "_")).Append("\n");

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
                    Material mat = mats[material];
                    string title = string.Format($"{mat.name}_{(int)(mat.color.r * 1000)}_{(int)(mat.color.g * 1000)}_{(int)(mat.color.b * 1000)}");
                    title = title.Replace("/", "_");
                    //title = mat.name;

                    sb.Append("\n");
                    sb.Append("usemtl ").Append(title).Append("\n");
                    sb.Append("usemap ").Append(title).Append("\n");
                    if (!MTLS.ContainsKey(title))
                    {
                        MTLS.Add(title, string.Format($"Kd {mat.color.r} {mat.color.g} {mat.color.b}").Replace(",", "."));
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