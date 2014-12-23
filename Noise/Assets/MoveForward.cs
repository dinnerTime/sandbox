using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour
{
	public float speed = 0.1f;

	void Update()
	{
		transform.position += Vector3.forward * speed;
	}
}
