using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

		var ring1 = createPoly(transform.position + Vector3.back, sides, radius * 0.5f);
		var ring2 = createPoly(transform.position, sides, radius);

        MeshStruct data = new MeshStruct();
        data.vertices = new Vector3[ring1.vertices.Length + ring2.vertices.Length];
        for(int i = 0; i < sides; i++)
        {
        	data.vertices[i] = ring1.vertices[i];
        }

        for(int i = sides; i < sides*2; i++)
        {
        	data.vertices[i] = ring2.vertices[i-sides];
        }

        data.uv = new Vector2[data.vertices.Length];

        int[] triangles = new int[sides * 2 * 3];

        for(int t = 0, v = 0; v < sides-1 /* x ringCount-1 */; t+=6, v++)
        {
        	triangles[t] = v;
        	triangles[t+1] = v+sides+1;
        	triangles[t+2] = v+sides;
        	triangles[t+3] = v;
        	triangles[t+4] = v+1;
        	triangles[t+5] = v+sides+1;
        }

        // Final two triangles
        triangles[triangles.Length-1] = sides - 1;
        triangles[triangles.Length-2] = (sides * 2)-1;
        triangles[triangles.Length-3] = sides;
        triangles[triangles.Length-4] = sides - 1;
        triangles[triangles.Length-5] = sides;
        triangles[triangles.Length-6] = 0;

        mesh.vertices = data.vertices;
		mesh.uv = data.uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	MeshStruct createPoly(Vector3 centerPoint, int sides, float r)
	{
		MeshStruct result = new MeshStruct();
		result.vertices = ShapeFactory.UnitRegular(centerPoint, sides, r);	
		result.uv = new Vector2[result.vertices.Length];
		result.normals = new Vector3[result.vertices.Length];

		for(int i = 0; i < result.vertices.Length; i++)
		{
			var v = result.vertices[i];
			result.uv[i] = new Vector2(v.x, v.y);
			result.normals[i] = Vector3.back;
		}

		return result;
	}


	[Range(1,32)]
	public int sides = 8;

	public float radius = 1f;
}

