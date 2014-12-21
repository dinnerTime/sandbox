using UnityEngine;
using System;
using System.Collections;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour
{

	private Mesh mesh;

#region noise
	/*public float frequency = 1f;
	
	[Range(1, 8)]
	public int octaves = 1;
	
	[Range(1f, 4f)]
	public float lacunarity = 2f;
	
	[Range(0f, 1f)]
	public float persistence = 0.5f;
	
	[Range(1, 3)]
	public int dimensions = 3;
	
	public NoiseMethodType type;
	
	public Gradient coloring;*/
#endregion

	[Range(1,200)]
	public int resolution = 0;

	private int currentResolution;

	private void OnEnable () {
		if (mesh == null) {
			mesh = new Mesh();
			mesh.name = "Surface Mesh";
			GetComponent<MeshFilter>().mesh = mesh;
		}
		Refresh();
	}

	public void Refresh()
	{
		if(currentResolution != resolution)
		{
			CreateGrid();
		}
	}

	void CreateGrid()
	{
		currentResolution = resolution;
		mesh.Clear();

		// Create vertices, normals, uv, and color values.
		int n = resolution + 1;
		int vertLength = n*n;
		Vector3[] vertices = new Vector3[vertLength];
		Vector3[] normals = new Vector3[vertLength];
		Vector2[] uv = new Vector2[vertLength]; 
		Color[] colors = new Color[vertLength];
		float stepSize = 1f / resolution;
		for(int v = 0, y = 0; y <= resolution; y++)
		{
			for(int x = 0; x <= resolution; x++, v++)
			{
				vertices[v] = new Vector3(x * stepSize - 0.5f, y * stepSize - 0.5f);
				normals[v] = Vector3.back;
				uv[v] = new Vector2(x * stepSize, y * stepSize);
				float r =  ((float)x)/resolution;
				float b =  ((float)y)/resolution;
				colors[v] = new Color(r,0,b,1);
			}
		}
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.colors = colors;

		// Create triangle values.
		int[] triangles = new int[resolution*resolution*6];
		for(int t = 0, y = 0, v = 0; y < resolution; y++, v++)
		{
			for(int x = 0; x < resolution; x++, v++, t += 6)
			{
				triangles[t] = v;
				triangles[t+1] = v + resolution + 1;
				triangles[t+2] = v + 1;
				triangles[t+3] = v + 1;
				triangles[t+4] = v + resolution + 1;
				triangles[t+5] = v + resolution + 2;

			}
		}
		mesh.triangles = triangles;
	}
}