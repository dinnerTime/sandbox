using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class Column : MonoBehaviour
{
	void OnEnable()
	{
		CreateMesh();
	}

	void CreateMesh()
	{
		/*var filter = GetComponent<MeshFilter>();
		var mesh = new Mesh();
		mesh.name = "column_mesh";
		filter.mesh = mesh;
		mesh.Clear();

		Vector3[] vertices = new Vector3[sides+1];
		vertices[0] = Vector3.zero;
		for(int i = 0; i < sides; i++)
		{

		};

		Vector2[] uvs = new Vector2[vertices.Length];
		Vector3[] normals = new Vector3[vertices.Length];
		for(int i = 0; i < vertices.Length; i++)
		{
			var v = vertices[i];
			uvs[i] = new Vector2(v.x, v.y);
			normals[i] = Vector3.back;
		}

		int[] triangles = new int[]
		{
			0,2,1,
			1,2,3
		};

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;*/
		//mesh.RecalculateNormals();
	}


	[Range(1,32)]
	public int sides = 8;

	public float radius = 1f;

	void OnDrawGizmos()
	{
		var vertices = Polygon.UnitRegular(transform.position, sides,radius);	
		Gizmos.color = Color.black;

		for(int i = 0; i < vertices.Length; i++)
		{
			Vector3 from = i == 0 ? vertices[vertices.Length-1] : vertices[i-1];
			Vector3 to = vertices[i];
			Gizmos.DrawLine(from,to);
		}
	}
}

