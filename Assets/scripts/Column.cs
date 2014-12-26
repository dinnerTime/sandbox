using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class Column : MonoBehaviour
{

	// Use this for initialization
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
		mesh.triangles = triangles;
		mesh.RecalculateBounds();*/
	}


	[Range(1,32)]
	public int sides = 8;

	void OnDrawGizmos()
	{
		Vector3[] vertices = new Vector3[8+1];
		//Vector3[] vertices = new Vector3[sides+1];
		vertices[0] = Vector3.zero;
		vertices[1] = Vector3.up;
		vertices[2] = Vector3.MoveTowards(Vector3.zero,Vector3.up + Vector3.right,1);
		vertices[3] = Vector3.right;
		vertices[4] = Vector3.MoveTowards(Vector3.zero,Vector3.right + Vector3.down,1);
		vertices[5] = Vector3.down;
		vertices[6] = Vector3.MoveTowards(Vector3.zero,Vector3.down + Vector3.left,1);
		vertices[7] = Vector3.left;
		vertices[8] = Vector3.MoveTowards(Vector3.zero,Vector3.left + Vector3.up,1);

		/*for(int i = 1; i < vertices.Length; i++)
		{
		}*/

		for(int i = 1; i < vertices.Length; i++)
		{
			Gizmos.DrawLine(vertices[i-1],vertices[i]);
		}
	}
}
