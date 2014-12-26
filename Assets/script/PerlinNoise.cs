using UnityEngine;

namespace noise
{
	public static class PerlinNoise
	{
		public static float Perlin1D (Vector3 point, float frequency)
		{
			point *= frequency;
			int i0 = Mathf.FloorToInt(point.x);
			float t0 = point.x - i0;
			float t1 = t0 -1f;

			i0 &= Noise.hashMask;
			int i1 = i0 + 1;

			float g0 = gradients1D[Noise.hash[i0] & gradientsMask1D];
			float g1 = gradients1D[Noise.hash[i1] & gradientsMask1D];

			float v0 = g0 * t0;
			float v1 = g1 * t1;

			float t = Noise.Smooth(t0);
			return Mathf.Lerp(v0,v1,t) * 2f;
		}

		public static float Perlin2D (Vector3 point, float frequency)
		{
			point *= frequency;
			int ix0 = Mathf.FloorToInt(point.x);
			int iy0 = Mathf.FloorToInt(point.y);
			float tx0 = point.x - ix0;
			float ty0 = point.y - iy0;
			float tx1 = tx0 - 1f;
			float ty1 = ty0 - 1f;

			ix0 &= Noise.hashMask;
			iy0 &= Noise.hashMask;
			int ix1 = ix0 + 1;
			int iy1 = iy0 + 1;

			int h0 = Noise.hash[ix0];
			int h1 = Noise.hash[ix1];

			Vector2 g00 = gradients2D[Noise.hash[h0 + iy0] & gradientsMask2D];
			Vector2 g10 = gradients2D[Noise.hash[h1 + iy0] & gradientsMask2D];
			Vector2 g01 = gradients2D[Noise.hash[h0 + iy1] & gradientsMask2D];
			Vector2 g11 = gradients2D[Noise.hash[h1 + iy1] & gradientsMask2D];

			float v00 = Dot(g00, tx0, ty0);
			float v10 = Dot(g10, tx1, ty0);
			float v01 = Dot(g01, tx0, ty1);
			float v11 = Dot(g11, tx1, ty1);

			float tx = Noise.Smooth(tx0);
			float ty = Noise.Smooth(ty0);

			return Mathf.Lerp(
				Mathf.Lerp(v00, v10, tx),
				Mathf.Lerp(v01, v11, tx),
				ty) * sqr2;
		}

		public static float Perlin3D (Vector3 point, float frequency)
		{
			point *= frequency;
			int ix0 = Mathf.FloorToInt(point.x);
			int iy0 = Mathf.FloorToInt(point.y);
			int iz0 = Mathf.FloorToInt(point.z);
			float tx0 = point.x - ix0;
			float ty0 = point.y - iy0;
			float tz0 = point.z - iz0;
			float tx1 = tx0 - 1f;
			float ty1 = ty0 - 1f;
			float tz1 = tz0 - 1f;
			ix0 &= Noise.hashMask;
			iy0 &= Noise.hashMask;
			iz0 &= Noise.hashMask;
			int ix1 = ix0 + 1;
			int iy1 = iy0 + 1;
			int iz1 = iz0 + 1;
			
			int h0 = Noise.hash[ix0];
			int h1 = Noise.hash[ix1];
			int h00 = Noise.hash[h0 + iy0];
			int h10 = Noise.hash[h1 + iy0];
			int h01 = Noise.hash[h0 + iy1];
			int h11 = Noise.hash[h1 + iy1];
			Vector3 g000 = gradients3D[Noise.hash[h00 + iz0] & gradientsMask3D];
			Vector3 g100 = gradients3D[Noise.hash[h10 + iz0] & gradientsMask3D];
			Vector3 g010 = gradients3D[Noise.hash[h01 + iz0] & gradientsMask3D];
			Vector3 g110 = gradients3D[Noise.hash[h11 + iz0] & gradientsMask3D];
			Vector3 g001 = gradients3D[Noise.hash[h00 + iz1] & gradientsMask3D];
			Vector3 g101 = gradients3D[Noise.hash[h10 + iz1] & gradientsMask3D];
			Vector3 g011 = gradients3D[Noise.hash[h01 + iz1] & gradientsMask3D];
			Vector3 g111 = gradients3D[Noise.hash[h11 + iz1] & gradientsMask3D];

			float v000 = Dot(g000, tx0, ty0, tz0);
			float v100 = Dot(g100, tx1, ty0, tz0);
			float v010 = Dot(g010, tx0, ty1, tz0);
			float v110 = Dot(g110, tx1, ty1, tz0);
			float v001 = Dot(g001, tx0, ty0, tz1);
			float v101 = Dot(g101, tx1, ty0, tz1);
			float v011 = Dot(g011, tx0, ty1, tz1);
			float v111 = Dot(g111, tx1, ty1, tz1);

			float tx = Noise.Smooth(tx0);
			float ty = Noise.Smooth(ty0);
			float tz = Noise.Smooth(tz0);
			return Mathf.Lerp(
				Mathf.Lerp(Mathf.Lerp(v000, v100, tx), Mathf.Lerp(v010, v110, tx), ty),
				Mathf.Lerp(Mathf.Lerp(v001, v101, tx), Mathf.Lerp(v011, v111, tx), ty),
				tz);
		}

		private static float[] gradients1D = {1f, -1f};

		private const int gradientsMask1D = 1;

		private static Vector2[] gradients2D = {
			new Vector2( 1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2( 0f, 1f),
			new Vector2( 0f,-1f),
			new Vector2( 1f, 1f).normalized,
			new Vector2(-1f, 1f).normalized,
			new Vector2( 1f,-1f).normalized,
			new Vector2(-1f,-1f).normalized
		};
	
		private const int gradientsMask2D = 7;

		private static float sqr2 = Mathf.Sqrt(2f);

		private static float Dot (Vector2 g, float x, float y) {
			return g.x * x + g.y * y;
		}

		private static float Dot (Vector3 g, float x, float y, float z) {
			return g.x * x + g.y * y + g.z * z;
		}

		private static Vector3[] gradients3D = {
		new Vector3( 1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3( 1f,-1f, 0f),
		new Vector3(-1f,-1f, 0f),
		new Vector3( 1f, 0f, 1f),
		new Vector3(-1f, 0f, 1f),
		new Vector3( 1f, 0f,-1f),
		new Vector3(-1f, 0f,-1f),
		new Vector3( 0f, 1f, 1f),
		new Vector3( 0f,-1f, 1f),
		new Vector3( 0f, 1f,-1f),
		new Vector3( 0f,-1f,-1f),
		
		new Vector3( 1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3( 0f,-1f, 1f),
		new Vector3( 0f,-1f,-1f)
	};
	
	private const int gradientsMask3D = 15;
	}
}