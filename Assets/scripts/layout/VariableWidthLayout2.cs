using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class VariableWidthLayout2 : MonoBehaviour
{
	public GameObject bluePrefab;
	public GameObject redPrefab;

    //int[] sequence = new int[] { 0,1,1,0,1 };
    //int[] sequence = new int[] { 0,1,1,1,0,1,1,1,1 };
    int[] sequence = new int[] {};
    readonly List<GameObject> items = new List<GameObject>();

    void OnEnable()
    {
    	updateLayout();
    }

    IEnumerator Start()
	{
		List<int> ints = new List<int>();

		int i = 0;
		while(true)
		{
			int zuz = 1;
			if(i == 0 || i % 4 == 0)
			{
				zuz = 0;
			}
			ints.Add(zuz);
			sequence = ints.ToArray();
			updateLayout();
			yield return new WaitForSeconds(1);
			i++;
		}
	}

	public float maxWidth = 8f;
	public float ew = 0.5f;
	public float cw = 1.5f;

	void updateLayout()
	{

		foreach(GameObject g in items) { GameObject.Destroy(g); }
		items.Clear();

		// Instantiate

		for(int i = 0; i < sequence.Length; i++)
		{
			int item = sequence[i];
			GameObject prefab = item == 0 ? redPrefab : bluePrefab;
			var instance = GameObject.Instantiate(prefab) as GameObject;
			items.Add(instance);
			instance.transform.parent = transform;
			instance.transform.position = Vector3.zero;
		}

		if(sequence.Length == 0) { return; }

		// Layout items

		float firstItemWidth = sequence[0] == 0 ? ew : cw;
		float lastItemWidth = sequence[sequence.Length-1] == 0 ? ew : cw;
		float min = transform.position.x - maxWidth*0.5f + (firstItemWidth * 0.5f);
		float max = transform.position.x + maxWidth*0.5f;
		float[] xArray = new float[sequence.Length];

		for(int i = 0; i < sequence.Length; i++)
		{
			float percent = i/(float)sequence.Length;
			xArray[i] = Mathf.Lerp(min,max,percent);
		}

		// Calculate excess (if the pile layout total exceeds the max width).
		int eCount = 0;
		int cCount = 0;
		for(int i = 0; i < sequence.Length; i++)
		{
			if(sequence[i] == 0) eCount++;
			else cCount++;
		}
		float totalWidth = (ew * eCount) + (cw * cCount);
		float excess = totalWidth - maxWidth;

		// Scoot card overlay if layout exceeds max width.
		if(excess > 0)
		{
			float subtractedWidth = excess/cCount; 

			float subtractRegister = 0f;
			for(int i = 0; i < sequence.Length; i++)
			{
				if(sequence[i] != 0)
				{
					subtractRegister -= subtractedWidth;
				    Debug.LogError("subtracting:"+subtractRegister);
				}

				xArray[i] += subtractRegister;
			}
	    }

		for(int i = 0; i < items.Count; i++)
		{
			items[i].transform.position += Vector3.right*xArray[i];
			float percent = i/(float)items.Count;
			float z = Mathf.Lerp(-0.5f,0.5f,percent);
			items[i].transform.position += Vector3.forward*z;
		}

		//TEMP
		/*var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
		c.transform.parent = transform; c.transform.position = Vector3.zero;
		c.transform.position += Vector3.right*min;
		c.transform.localScale = new Vector3(0.2f,1f,0.2f);
		var c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		c2.transform.parent = transform; c2.transform.position = Vector3.zero;
		c2.transform.localScale = new Vector3(0.2f,1f,0.2f);
		c2.transform.position += Vector3.right*max;*/
	}
}
