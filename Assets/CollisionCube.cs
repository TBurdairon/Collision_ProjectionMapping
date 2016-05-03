using UnityEngine;
using System.Collections;

public class CollisionCube : MonoBehaviour 
{
	//public float colourChangeDelay = 0.5f;
//	float currentDelay = 0f;
	public bool colourChangeCollision = false;
	public GameObject	movableCube;
	//public Camera cam;
	static int instanciated = 0;
	int cnt;

	void Start()
	{
		++instanciated;
		cnt = instanciated;
		movableCube.GetComponent<MeshRenderer>().material.color = new Color(instanciated *0.1f, instanciated * 0.1f, 0f);
	}
		

	void OnTriggerStay(Collider other) {
	//	Debug.Log("Contact was made!");
		colourChangeCollision = true;
	//	currentDelay = Time.time + colourChangeDelay;
	}

	void OnTriggerExit(Collider other)
	{
		colourChangeCollision = false;
	//	Debug.Log ("false");
	}




}
