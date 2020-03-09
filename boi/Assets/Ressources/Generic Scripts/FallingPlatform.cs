using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

	private Rigidbody2D rb;
	public GameObject object1;
	public GameObject object2;


	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D rb)
	{
		if (rb.gameObject.name.Equals("Player"))
		{
			Invoke("DropPlatform", 0.5f);
			Destroy(gameObject, 2f);
		}
	}


	void DropPlatform()
	{
		rb.isKinematic = false;
	}
}
