using UnityEngine;
using System.Collections;
using noise;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public class TextureCreator : MonoBehaviour
{
	[Range(2,512)]
	public int resolution = 256;

	[Range(1,3)]
	public int dimensions = 3;

	public float frequency = 1f;

	public NoiseMethodType type;

	private Texture2D texture;

	private void OnEnable()
	{
		texture = new Texture2D(resolution,resolution, TextureFormat.RGB24, false);
		texture.name = "Procedural texture";
		GetComponent<MeshRenderer>().material.mainTexture = texture;
		FillTexture();
	}

	void Update()
	{
		if(transform.hasChanged)
		{
			transform.hasChanged = false;
			FillTexture();
		}
	}

	public void FillTexture()
	{
		if(texture.width != resolution)
		{
			texture.Resize(resolution,resolution);
		}

		float half = 0.5f;
		Vector3 point00 = transform.TransformPoint(new Vector3(-half,-half));
		Vector3 point10 = transform.TransformPoint(new Vector3(half,-half));
		Vector3 point01 = transform.TransformPoint(new Vector3(-half,half));
		Vector3 point11 = transform.TransformPoint(new Vector3(half,half));

		Random.seed = 42;

		NoiseMethod noiseMethod = Noise.noiseMethods[(int)type][dimensions - 1];
		float stepsize = 1f / resolution;
		for(int y = 0; y < resolution; y++)
		{
			Vector3 point0 = Vector3.Lerp(point00,point01,(y + 0.5f) * stepsize);
			Vector3 point1 = Vector3.Lerp(point10,point11,(y + 0.5f) * stepsize);

			for(int x = 0; x < resolution; x++)
			{
				Vector3 point = Vector3.Lerp(point0,point1,(x + 0.5f) * stepsize);
				texture.SetPixel(x, y, Color.white * noiseMethod(point, frequency));
			}
		}
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Point;
        //texture.anisoLevel = 9;
		texture.Apply();
	}
}
