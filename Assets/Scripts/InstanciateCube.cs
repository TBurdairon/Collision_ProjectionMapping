using UnityEngine;
using System.Collections;

public class InstanciateCube : MonoBehaviour 
{
	public GameObject Hand;
	public Camera mainCamera;
	public Camera secondCamera;
	public EventManager eventMger;

	public int numberOfCar;
	private int cntCars= 0;

	// Use this for initialization

	void Start()
	{
		mainCamera.enabled = true;
		secondCamera.enabled = false;
	}

	public void InstanciateDefaultCube()
	{
		++cntCars;
		if (cntCars <= numberOfCar) 
		{
			//Debug.Log ("hey");
			GameObject go = (GameObject)Instantiate (Resources.Load ("Cube"));
			GameObject mc = (GameObject)Instantiate (Resources.Load ("MovableCube"));
			mc.transform.rotation = new Quaternion (0, 0, 0, 0);
			//mc.transform.position.Set (x + cntCars, y, z);
			if (cntCars == 1) //Main
			{
				go.GetComponent<BoxCollider>().enabled = false;
				((PlayVideo)mc.GetComponent (typeof(PlayVideo))).state = PlayVideo.State.Main;
			}
			CollisionCube changeColor = (CollisionCube)go.GetComponent (typeof(CollisionCube));
			changeColor.movableCube = mc;
			go.transform.position = Hand.transform.position;
			mc.gameObject.SetActive (false);
			eventMger.addCubes (go);
		} 
		if (cntCars >= numberOfCar)
		{
			mainCamera.enabled = false;
			secondCamera.enabled = true;
			if (cntCars == numberOfCar)
				eventMger.StartGame ();
		}	
	}
}
