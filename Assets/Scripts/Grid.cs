using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int xSize, ySize;
	public SelectionManager selectionManager;

	private Mesh mesh;
	private Vector3[] vertices;
	private Transform childStatic;
	private Transform childDyn;
	private Transform[] childchildDyn = new Transform[4];
	private Transform[] childchildStatic = new Transform[4];
	private GameObject tmp;


	void Start () 
	{
		Generate ();
	}


	void Update()
	{
		if (transform.rotation.x != 0.0f)
			transform.rotation = new Quaternion (0, 0, 0, 0);
	}
	

	private void Generate () 
	{
		GameObject tmp = new GameObject ();
		transform.GetChild (0).parent = tmp.transform;
		transform.GetChild (0).parent = tmp.transform;
		transform.position = new Vector3 (transform.position.x - 0.125f, transform.position.y - 0.125f, transform.position.z);
		transform.localScale = new Vector3 (transform.localScale.x / xSize , transform.localScale.y / ySize,  transform.localScale.y / ySize);
		tmp.transform.GetChild(0).parent = transform;
		tmp.transform.GetChild(0).parent = transform;
		Destroy (tmp);
		
		//DetachChild ();
	//	ScaleChild ();
			//ScaleChild ();
		//retachChild ();
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y);
				uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
				tangents[i] = tangent;
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;

		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		selectionManager.startHomography = true;
	//	ScaleChild ();
		//transform.position = new Vector3 (transform.position.x - 0.125f, transform.position.y - 0.125f, 0f);

		transform.rotation = new Quaternion (0, 0, 0, 0);

	}
}