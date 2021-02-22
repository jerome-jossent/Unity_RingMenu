using UnityEngine;

public class MousePos2Shader : MonoBehaviour
{
	private float radius = 2;
	private RaycastHit hit;
	private Ray ray;

	void Update()
	{
		// get mouse pos
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			GetComponent<Renderer>().material.SetVector("_ObjPos", new Vector4(hit.point.x, hit.point.y, hit.point.z, 0));
		}

		// box rotation for testing
		if (Input.GetKey("a"))
		{
			transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
		}
		if (Input.GetKey("d"))
		{
			transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
		}

		// mousewheel for radius
		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			radius += (float)Input.GetAxis("Mouse ScrollWheel") * 0.8f;
			GetComponent<Renderer>().material.SetFloat("_Radius", radius);
		}
	}
}
