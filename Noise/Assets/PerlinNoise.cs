using UnityEngine;

namespace noise
{
	public static class PerlinNoise
	{
		public static float Perlin1D (Vector3 point, float frequency)
		{
			point *= frequency;
			int i0 = Mathf.FloorToInt(point.x);
			float t = point.x - i0;
			t = Noise.Smooth(t);
			i0 &= Noise.hashMask;
			int i1 = i0 + 1;

			int h0 = Noise.hash[i0];
			int h1 = Noise.hash[i1];

			return Mathf.Lerp(h0,h1,t) * (1f / Noise.hashMask);
		}

		public static float Perlin2D (Vector3 point, float frequency)
		{
			point *= frequency;
			int ix0 = Mathf.FloorToInt(point.x);
			int iy0 = Mathf.FloorToInt(point.y);
			float tx = point.x - ix0;
			float ty = point.y - iy0;
			ix0 &= Noise.hashMask;
			iy0 &= Noise.hashMask;
			int ix1 = ix0 + 1;
			int iy1 = iy0 + 1;

			int h0 = Noise.hash[ix0];
			int h1 = Noise.hash[ix1];
			int h00 = Noise.hash[h0 + iy0];
			int h10 = Noise.hash[h1 + iy0];
			int h01 = Noise.hash[h0 + iy1];
			int h11 = Noise.hash[h1 + iy1];

			tx = Noise.Smooth(tx);
			ty = Noise.Smooth(ty);

			return Mathf.Lerp(
				Mathf.Lerp(h00, h10, tx),
				Mathf.Lerp(h01, h11, tx),
				ty) * (1f / Noise.hashMask);
		}

		public static float Perlin3D (Vector3 point, float frequency)
		{
			point *= frequency;
			int ix0 = Mathf.FloorToInt(point.x);
			int iy0 = Mathf.FloorToInt(point.y);
			int iz0 = Mathf.FloorToInt(point.z);
			float tx = point.x - ix0;
			float ty = point.y - iy0;
			float tz = point.z - iz0;
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
			int h000 = Noise.hash[h00 + iz0];
			int h100 = Noise.hash[h10 + iz0];
			int h010 = Noise.hash[h01 + iz0];
			int h110 = Noise.hash[h11 + iz0];
			int h001 = Noise.hash[h00 + iz1];
			int h101 = Noise.hash[h10 + iz1];
			int h011 = Noise.hash[h01 + iz1];
			int h111 = Noise.hash[h11 + iz1];

			tx = Noise.Smooth(tx);
			ty = Noise.Smooth(ty);
			tz = Noise.Smooth(tz);

			return Mathf.Lerp(
				Mathf.Lerp(Mathf.Lerp(h000, h100, tx), Mathf.Lerp(h010, h110, tx), ty),
				Mathf.Lerp(Mathf.Lerp(h001, h101, tx), Mathf.Lerp(h011, h111, tx), ty),
				tz) * (1f / Noise.hashMask);
		}
	}
}