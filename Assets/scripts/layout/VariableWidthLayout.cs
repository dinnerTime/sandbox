using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class VariableWidthLayout : MonoBehaviour
{
	public GameObject bluePrefab;
	public GameObject redPrefab;

	bool updated = false;

    int[] sequence = new int[] { 0,1,1,1,0,1 };
    readonly List<GameObject> items = new List<GameObject>();

    void OnEnable()
    {
    	updateLayout();
    }

	void Update()
	{
		if(!updated) { updateLayout(); updated = true; }
	}


	public float maxWidth = 8f;
	float pilePadding = 0.25f;
	float cardOverlay = 0.25f;
	public float ew = 0.5f;
	public float cw = 1.5f;

	void updateLayout()
	{

		Debug.Log("updateLayout");//TEMP
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

		// Layout items

		float xRegister = transform.position.x - maxWidth*0.5f;
		float[] xArray = new float[sequence.Length];
		for(int i = 0; i < sequence.Length; i++)
		{
			int item = sequence[i];
			float w = item == 0 ? ew : cw;

			// Add half the width of the current renderer.
			xRegister += w * 0.5f;

			xArray[i] = xRegister;

		    // Add the second half to the register.
			xRegister += w * 0.5f;

			// Add margin as needed
			float margin = 0f;
			if(i+1 < sequence.Length)
			{
				int nextItem = sequence[i+1];
				margin += nextItem == 0 ? pilePadding : cardOverlay;
			}
			xRegister += margin;
		}

		for(int i = 0; i < items.Count; i++)
		{
			float percent = i/(float)items.Count;
			float z = Mathf.Lerp(-0.5f,0.5f,percent);
			items[i].transform.position += Vector3.right*xArray[i];
			//items[i].transform.position += Vector3.forward*z;
		}
	}
}
