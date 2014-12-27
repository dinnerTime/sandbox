using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Column : MonoBehaviour
{
	int cachedSides = -1;

	void Update()
	{
		if(sides != cachedSides)
		{
			cachedSides = sides;
			CreateMesh();
		}
	}

	void OnEnable()
	{
		CreateMesh();
	}

	void CreateMesh()
	{
		var filter = GetComponent<MeshFilter>();
		var mesh = new Mesh();
		mesh.name = "column_mesh";
		filter.mesh = mesh;
		mesh.Clear();

        Vector3[] vertices = new Vector3[sides+1];
        vertices[0] = transform.position;

		Vector3[] outerVertices = ShapeFactory.UnitRegular(transform.position, sides, radius);	
		for(int i = 1; i < vertices.Length; i++)
		{
			vertices[i] = outerVertices[i-1];
		}

		Vector2[] uvs = new Vector2[vertices.Length];
		Vector3[] normals = new Vector3[vertices.Length];

		for(int i = 0; i < vertices.Length; i++)
		{
			var v = vertices[i];
			uvs[i] = new Vector2(v.x, v.y);
			normals[i] = Vector3.back;
		}

		int[] triangles = new int[sides*3];

		// Fill all but final triangle.
		int tri = 0;
		for(int t = 0; t < triangles.Length - 3; t+=3)
		{
			triangles[t] = 0;
			triangles[t+1] = tri+2;
			triangles[t+2] = tri+1;
			tri++;
		}

		// Final triangle.
		triangles[triangles.Length - 1] = 0;
		triangles[triangles.Length - 2] = sides;
		triangles[triangles.Length - 3] = 1;

		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}


	[Range(1,32)]
	public int sides = 8;

	public float radius = 1f;

	/*void OnDrawGizmos()
	{
		var vertices = Polygon.UnitRegular(transform.position, sides,radius);	
		Gizmos.color = Color.black;

		for(int i = 0; i < vertices.Length; i++)
		{
			Vector3 from = i == 0 ? vertices[vertices.Length-1] : vertices[i-1];
			Vector3 to = vertices[i];
			Gizmos.DrawLine(from,to);
		}
	}*/
}

