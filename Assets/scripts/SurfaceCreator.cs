using UnityEngine;
using System;
using System.Collections;
using noise;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour
{

	private Mesh mesh;

	public float frequency = 1f;

	public Vector3 rotation = Vector3.zero;
	
	[Range(1, 8)]
	public int octaves = 1;
	
	[Range(1f, 4f)]
	public float lacunarity = 2f;
	
	[Range(0f, 1f)]
	public float persistence = 0.5f;
	
	[Range(1, 3)]
	public int dimensions = 3;

	public float maxHeight = 1f;
	
	public NoiseMethodType type;
	
	public Gradient coloring;

	private Color[] colors;
	private Vector3[] vertices;

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
		MakeNoise();
	}

	void CreateGrid()
	{
		currentResolution = resolution;
		mesh.Clear();

		// Create vertices, normals, uv, and color values.
		int n = resolution + 1;
		int vertLength = n*n;
		this.vertices = new Vector3[vertLength];
		Vector3[] normals = new Vector3[vertLength];
		Vector2[] uv = new Vector2[vertLength]; 
		colors = new Color[vertLength];
		float stepSize = 1f / resolution;
		for (int v = 0, z = 0; z <= resolution; z++) {
			for (int x = 0; x <= resolution; x++, v++) {
				vertices[v] = new Vector3(x * stepSize - 0.5f, 0f, z * stepSize - 0.5f);
				colors[v] = Color.black;
				normals[v] = Vector3.up;
				uv[v] = new Vector2(x * stepSize, z * stepSize);
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

		mesh.vertices = vertices;
		mesh.triangles = triangles;
	}

	void MakeNoise()
	{
		Quaternion q = Quaternion.Euler(rotation);
		Vector3 point00 = q * transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = q * transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = q * transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = q * transform.TransformPoint(new Vector3( 0.5f, 0.5f));
		
		NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1];
		float stepSize = 1f / resolution;
		for (int v = 0, y = 0; y <= resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, y * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, y * stepSize);
			for (int x = 0; x <= resolution; x++, v++) {
				Vector3 point = Vector3.Lerp(point0, point1, x * stepSize);
				float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
			    sample = type == NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);	
				if (type != NoiseMethodType.Value) {
					sample = sample * 0.5f + 0.5f;
				}
				colors[v] = coloring.Evaluate(sample);
				vertices[v].y = Mathf.Lerp(0,maxHeight,sample);
			}
		}

		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.RecalculateNormals();
	}
}