using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTester : MonoBehaviour
{
	GameObject[][] renderedChunks;

	int cachedCount = -1;

	[Range(1,32)]
	public int chunkCount = 3;

	public GameObject prefab;

	float size = 10.1f;

	void recalculate()
	{
		if(renderedChunks != null)
		{
			foreach(GameObject[] gArray in renderedChunks)
			{
				foreach(GameObject g in gArray)
				{
				  GameObject.Destroy(g);
				}
			}
		}

		renderedChunks = new GameObject[chunkCount][];
		for(int x = 0; x < chunkCount; x++)
		{
			GameObject[] row = new GameObject[chunkCount];
			renderedChunks[x] = row;
			for(int z = 0; z < chunkCount; z++)
			{
				var plane = GameObject.Instantiate(prefab) as GameObject;
				float halfSize = (size*chunkCount)/2;
				float xPos = Mathf.Lerp(-halfSize,halfSize,x/(float)chunkCount);
				float zPos = Mathf.Lerp(-halfSize,halfSize,z/(float)chunkCount);
				plane.transform.position = new Vector3(xPos,transform.position.y, zPos);
				plane.transform.parent = transform;
				row[z] = plane;
			}
		}
	}

	void Update()
	{
		if(cachedCount != chunkCount)
		{
			cachedCount = chunkCount;
			recalculate();
		}
	}

	void OnEnable()
	{
		recalculate();
	}
}