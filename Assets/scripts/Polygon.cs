using UnityEngine;
using System.Collections;

public static class Polygon 
{
	public static Vector3[] UnitRegular(Vector3 center, int sides, float radius)
	{
		Vector3[] vertices = new Vector3[sides];

		float radian = (Mathf.PI*2f)/(float)sides;

		for(int i = 0; i < vertices.Length; i++)
		{
			Vector3 unit = Vector3.up;
			Vector3 v = rotateVector(unit,radian*i, radius);
			v += center;
			vertices[i] = v;
		}

		return vertices;
	}

	static Vector3 rotateVector(Vector3 v, float radians, float radius)
	{
		float newX = v.x + Mathf.Cos(radians) - v.y * Mathf.Sin(radians);
		newX = newX * radius;
		float newY = v.x + Mathf.Sin(radians) + v.y * Mathf.Cos(radians);
		newY = newY * radius;
		Vector3 result = new Vector3(newX,newY,0f);
		return result;
	}
}
