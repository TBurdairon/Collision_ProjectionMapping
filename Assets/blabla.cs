using UnityEngine;
using System.Collections;

public class blabla : MonoBehaviour {

	// Use this for initialization
	public MovieTexture b;

	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.L)) 
		{
			Renderer a = GetComponent<Renderer> ();
			a.material.mainTexture = b;
			b.Play ();
		}
	
	}
}
