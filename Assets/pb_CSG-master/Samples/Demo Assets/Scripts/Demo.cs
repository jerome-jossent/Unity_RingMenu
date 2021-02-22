using UnityEngine;
using Parabox.CSG;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Parabox.CSG.Demo
{
	/// <summary>
	/// Simple demo of CSG operations.
	/// </summary>
	public class Demo : MonoBehaviour
	{
		GameObject left, right, composite;
		bool wireframe = false;

		public Material wireframeMaterial = null;

		public GameObject[] fodder; // prefabs containing two mesh children
		int index = 0; // the index of example mesh prefabs

		enum BoolOp
		{
			Union,
			SubtractLR,
			SubtractRL,
			Intersect
		};

		ScriptsManager _sm;

		private void Awake()
		{
			_sm = GameObject.Find("ScriptsManager").GetComponent<ScriptsManager>();
			//}

			//void Awake()
			//{
			Reset();

			wireframeMaterial.SetFloat("_Opacity", 0);
			cur_alpha = 0f;
			dest_alpha = 0f;

			ToggleWireframe();
		}

		private void Start()
		{
			if (composite) Destroy(composite);
			if (left) Destroy(left);
			if (right) Destroy(right);

			

			left = Triangle3DPrimitive.CreateTriangle();
			//MeshExporter.ObjExporter.MeshToFile(left, $"Triangle3DPrimitiveJJ.obj");
			//left = MakeCylinder(1, 1);
		}
		
		/// <summary>
		/// Reset the scene to it's original state.
		/// </summary>
		public void Reset()
		{
			if (composite) Destroy(composite);
			if (left) Destroy(left);
			if (right) Destroy(right);

			var go = Instantiate(fodder[index]);

			left = Instantiate(go.transform.GetChild(0).gameObject);
			right = Instantiate(go.transform.GetChild(1).gameObject);

			Destroy(go);

			wireframeMaterial = left.GetComponent<MeshRenderer>().sharedMaterial;

			GenerateBarycentric(left);
			GenerateBarycentric(right);
		}

		public void Union()
		{
			Reset();
			DoBooleanOperation(BoolOp.Union);
		}

		public void SubtractionLR()
		{
			Reset();
			DoBooleanOperation(BoolOp.SubtractLR);
		}

		public void SubtractionRL()
		{
			Reset();
			DoBooleanOperation(BoolOp.SubtractRL);
		}

		public void Intersection()
		{
			Reset();
			DoBooleanOperation(BoolOp.Intersect);
		}

		GameObject CreateRing()
		{
			//GameObject secteur1 = Sector3D.CreateObject(1, 2, 0, 90, 18, "S1");
			//GameObject secteur2 = Sector3D.CreateObject(1, 2, 90, 180, 18, "S2");
			//GameObject secteur3 = Sector3D.CreateObject(1, 2, 180, 270, 18, "S3");
			//GameObject secteur4 = Sector3D.CreateObject(1, 2, 270, 360, 18, "S4");
			GameObject secteur1 = Sector3D.CreateObject(1, 2, 0, 90, name: "S1");
			GameObject secteur2 = Sector3D.CreateObject(1, 2, 90, 180, name: "S2");
			GameObject secteur3 = Sector3D.CreateObject(1, 2, 180, 270, name: "S3");
			GameObject secteur4 = Sector3D.CreateObject(1, 2, 270, 360, name: "S4");

			MaterialSetColor.Colorier(secteur1, new Color(1f, 0f, 0f, 0.5f));
			MaterialSetColor.Colorier(secteur2, new Color(1f, 1f, 0f, 0.5f));
			MaterialSetColor.Colorier(secteur3, new Color(0f, 0f, 1f, 0.5f));
			MaterialSetColor.Colorier(secteur4, new Color(0f, 1f, 1f, 0.5f));

			secteur1.AddComponent<ClickOnCollider>();
			secteur2.AddComponent<ClickOnCollider>();
			secteur3.AddComponent<ClickOnCollider>();
			secteur4.AddComponent<ClickOnCollider>();

			//CSG_Model csg_model_1 = new CSG_Model(secteur1);
			//CSG_Model csg_model_2 = new CSG_Model(secteur2);
			//CSG_Model csg_model_3 = new CSG_Model(secteur3);
			//CSG_Model csg_model_4 = new CSG_Model(secteur4);

			//CSG_Node a1 = new CSG_Node(csg_model_1.ToPolygons());
			//CSG_Node a2 = new CSG_Node(csg_model_2.ToPolygons());
			//CSG_Node a3 = new CSG_Node(csg_model_3.ToPolygons());
			//CSG_Node a4 = new CSG_Node(csg_model_4.ToPolygons());

			//List<CSG_Polygon> polygons12 = CSG_Node.Union(a1, a2).AllPolygons();
			//CSG_Model result12 = new CSG_Model(polygons12);
			//CSG_Node a12 = new CSG_Node(result12.ToPolygons());

			//List<CSG_Polygon> polygons123 = CSG_Node.Union(a12, a3).AllPolygons();
			//CSG_Model result123 = new CSG_Model(polygons123);
			//CSG_Node a123 = new CSG_Node(result123.ToPolygons());

			//List<CSG_Polygon> polygons1234 = CSG_Node.Union(a123, a4).AllPolygons();
			//CSG_Model result1234 = new CSG_Model(polygons1234);
			////CSG_Node a1234 = new CSG_Node(result1234.ToPolygons());



			GameObject secteurs = new GameObject();

			secteur1.transform.parent = secteurs.transform;
			secteur2.transform.parent = secteurs.transform;
			secteur3.transform.parent = secteurs.transform;
			secteur4.transform.parent = secteurs.transform;

			//secteurs.AddComponent<MeshFilter>().sharedMesh = result12.mesh;
			//secteurs.AddComponent<MeshRenderer>().sharedMaterials = result12.materials.ToArray();

			return secteurs;

			//float r_ext = 1;
			//float r_int = 0.8f;
			//float epaisseur = 0.02f;

			//GameObject Anneau_Exterieur = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			//GameObject Anneau_Interieur = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			//Anneau_Exterieur.transform.localScale = new Vector3(r_ext, epaisseur, r_ext);
			//Anneau_Interieur.transform.localScale = new Vector3(r_int, epaisseur + 1, r_int);

			//wireframeMaterial = Anneau_Exterieur.GetComponent<MeshRenderer>().sharedMaterial;

			//GenerateBarycentric(Anneau_Exterieur);
			//GenerateBarycentric(Anneau_Interieur);

			//CSG_Model anneau = Boolean.Subtract(Anneau_Exterieur, Anneau_Interieur);

			//Destroy(Anneau_Exterieur);
			//Destroy(Anneau_Interieur);

			//GameObject Anneau = new GameObject();
			//Anneau.AddComponent<MeshFilter>().sharedMesh = anneau.mesh;
			//Anneau.AddComponent<MeshRenderer>().sharedMaterials = anneau.materials.ToArray();

			//GenerateBarycentric(Anneau);

			//return Anneau;
		}


		#region CYLINDRE JJ =============================
		GameObject MakeCylinder(float diameter, float height, int numOfPointsOnDisks = 36)
		{
			var obj = new GameObject("Cylindre");
			var mesh = MakeCylinderMesh(diameter, height, numOfPointsOnDisks);
			var filter = obj.AddComponent<MeshFilter>();
			var renderer = obj.AddComponent<MeshRenderer>();
			var collider = obj.AddComponent<MeshCollider>();

			filter.sharedMesh = mesh;
			collider.sharedMesh = mesh;
			renderer.sharedMaterial = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

			return obj;
		}

		public Mesh MakeCylinderMesh(float diameter, float height, int numOfPointsOnDisks)
		{
			float radius = diameter / 2;
			#region Make Disk 1
			//https://answers.unity.com/questions/944228/creating-a-smooth-round-flat-circle.html
			float angleStep = 360.0f / (float)numOfPointsOnDisks;
			List<Vector3> vertexList = new List<Vector3>();
			List<int> triangleList = new List<int>();
			Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
			// Make first triangle.
			vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
			vertexList.Add(new Vector3(0.0f, radius, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
			vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
															// Add triangle indices.
			triangleList.Add(0);
			triangleList.Add(1);
			triangleList.Add(2);
			for (int i = 0; i < numOfPointsOnDisks - 1; i++)
			{
				triangleList.Add(0);                      // Index of circle center.
				triangleList.Add(vertexList.Count - 1);
				triangleList.Add(vertexList.Count);
				vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
			}
			Mesh mesh = new Mesh();
			mesh.vertices = vertexList.ToArray();
			mesh.triangles = triangleList.ToArray();
			#endregion

			#region Make Disk 2

			#endregion

			#region Make Enveloppe

			#endregion

			return mesh;
		}
		#endregion

		public void JJ_toOBJFile()
		{
			GameObject go = _sm._gameObjectSelectionManager._Selection;
			MeshExporter.ObjExporter.MeshToFile(go, go.name + ".obj");
		}

		public void JJCompute()
		{
			//            Triangle3DPrimitive.CreateTriangle();

			if (composite) Destroy(composite);
			if (left) Destroy(left);
			if (right) Destroy(right);

			composite = CreateRing();
			return;


			//composite.AddComponent<MeshCollider>();
			//composite.AddComponent<ClickOnCollider>();

			GameObject triangle3d = Triangle3DPrimitive.CreateTriangle();
			//GameObject triangle2d = TrianglePrimitive.CreateTriangle();
			//triangle2d.transform.Rotate(Vector3.right, -90);
			//triangle2d.transform.Translate(Vector3.up * 0.02f, Space.World);

			CSG_Model csg_model_a = new CSG_Model(composite);
			CSG_Model csg_model_b = new CSG_Model(triangle3d);

			CSG_Node a = new CSG_Node(csg_model_a.ToPolygons());
			CSG_Node b = new CSG_Node(csg_model_b.ToPolygons());

			List<CSG_Polygon> polygons = CSG_Node.Intersect(a, b).AllPolygons();

			CSG_Model result2 = new CSG_Model(polygons);

			Destroy(composite);
			Destroy(triangle3d);

			GameObject composite2 = new GameObject();
			composite2.AddComponent<MeshFilter>().sharedMesh = result2.mesh;
			composite2.AddComponent<MeshRenderer>().sharedMaterials = result2.materials.ToArray();

			GenerateBarycentric(composite2);

			composite2.AddComponent<MeshCollider>();
			composite2.AddComponent<ClickOnCollider>();
		}

		void DoBooleanOperation(BoolOp operation)
		{
			CSG_Model result;

			/**
			 * All boolean operations accept two gameobjects and return a new mesh.
			 * Order matters - left, right vs. right, left will yield different
			 * results in some cases.
			 */
			switch (operation)
			{
				case BoolOp.Union:
					result = Boolean.Union(left, right);
					break;

				case BoolOp.SubtractLR:
					result = Boolean.Subtract(left, right);
					break;

				case BoolOp.SubtractRL:
					result = Boolean.Subtract(right, left);
					break;

				default:
					result = Boolean.Intersect(right, left);
					break;
			}

			composite = new GameObject();
			composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
			composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

			GenerateBarycentric(composite);

			Destroy(left);
			Destroy(right);

			composite.AddComponent<MeshCollider>();
			composite.AddComponent<ClickOnCollider>();

			//TrianglePrimitive.CreateTriangle();

		}

		/// <summary>
		/// Turn the wireframe overlay on or off.
		/// </summary>
		public void ToggleWireframe()
		{
			wireframe = !wireframe;

			cur_alpha = wireframe ? 0f : 1f;
			dest_alpha = wireframe ? 1f : 0f;
			start_time = Time.time;
		}

		/// <summary>
		/// Swap the current example meshes
		/// </summary>
		public void ToggleExampleMeshes()
		{
			index++;
			if (index > fodder.Length - 1) index = 0;

			Reset();
		}

		float wireframe_alpha = 0f, cur_alpha = 0f, dest_alpha = 1f, start_time = 0f;

		void Update()
		{
			wireframe_alpha = Mathf.Lerp(cur_alpha, dest_alpha, Time.time - start_time);
			wireframeMaterial.SetFloat("_Opacity", wireframe_alpha);
		}

		/**
		 * Rebuild mesh with individual triangles, adding barycentric coordinates
		 * in the colors channel.  Not the most ideal wireframe implementation,
		 * but it works and didn't take an inordinate amount of time :)
		 */
		void GenerateBarycentric(GameObject go)
		{
			Mesh m = go.GetComponent<MeshFilter>().sharedMesh;

			if (m == null) return;

			int[] tris = m.triangles;
			int triangleCount = tris.Length;

			Vector3[] mesh_vertices = m.vertices;
			Vector3[] mesh_normals = m.normals;
			Vector2[] mesh_uv = m.uv;

			Vector3[] vertices = new Vector3[triangleCount];
			Vector3[] normals = new Vector3[triangleCount];
			Vector2[] uv = new Vector2[triangleCount];
			Color[] colors = new Color[triangleCount];

			for (int i = 0; i < triangleCount; i++)
			{
				vertices[i] = mesh_vertices[tris[i]];
				normals[i] = mesh_normals[tris[i]];
				uv[i] = mesh_uv[tris[i]];

				colors[i] = i % 3 == 0 ? new Color(1, 0, 0, 0) : (i % 3) == 1 ? new Color(0, 1, 0, 0) : new Color(0, 0, 1, 0);

				tris[i] = i;
			}

			Mesh wireframeMesh = new Mesh();

			wireframeMesh.Clear();
			wireframeMesh.vertices = vertices;
			wireframeMesh.triangles = tris;
			wireframeMesh.normals = normals;
			wireframeMesh.colors = colors;
			wireframeMesh.uv = uv;

			go.GetComponent<MeshFilter>().sharedMesh = wireframeMesh;
		}















		#region Mesh to Disk




		//public static string MeshToString(MeshFilter mf)
		//public static string MeshToString(GameObject go)
		//{
		//	MeshFilter mf = go.GetComponent<MeshFilter>();
		//	Mesh m = mf.mesh;
		//	Renderer rd = go.GetComponent<Renderer>();
		//	Material[] mats = rd.sharedMaterials;

		//	StringBuilder sb = new StringBuilder();

		//	sb.Append("g ").Append(mf.name).Append("\n");
		//	foreach (Vector3 v in m.vertices)
		//	{
		//		sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z).Replace(",","."));
		//	}
		//	sb.Append("\n");
		//	foreach (Vector3 v in m.normals)
		//	{
		//		sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z).Replace(",", "."));
		//	}
		//	sb.Append("\n");
		//	foreach (Vector3 v in m.uv)
		//	{
		//		sb.Append(string.Format("vt {0} {1}\n", v.x, v.y).Replace(",", "."));
		//	}
		//	for (int material = 0; material < m.subMeshCount; material++)
		//	{
		//		sb.Append("\n");
		//		sb.Append("usemtl ").Append(mats[material].name).Append("\n");
		//		sb.Append("usemap ").Append(mats[material].name).Append("\n");

		//		int[] triangles = m.GetTriangles(material);
		//		for (int i = 0; i < triangles.Length; i += 3)
		//		{
		//			sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
		//				triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
		//		}
		//	}
		//	return sb.ToString();
		//}

		////public static void MeshToFile(MeshFilter mf, string filename)
		//public static void MeshToFile(GameObject go, string filename)
		//{
		//	using (StreamWriter sw = new StreamWriter(filename))
		//	{
		//		sw.Write(MeshToString(go));
		//	}
		//}



		#endregion

	}
}
