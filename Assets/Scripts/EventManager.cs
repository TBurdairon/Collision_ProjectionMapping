using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventManager : MonoBehaviour 
{
	public List<GameObject> cubes;

//	public List<GameObject> hand;
	public GameObject bones;
	public GameObject Hand;
	public float distanceActivate;
	public GameObject ZoneActivate;
	public ParseFile parseFile;
	public Button button;
	public int NumberOfClips;
	public CommunicationArduino comArduino;


	private PlayVideo mainVideo;
	private int[] value;
	private bool[] end;
	private bool[] go;

	//private GameObject currentGameObject;
	//private GameObject previousGameObject;
	private Vector3 averageCubesDistance;
	private int tmpValue;
	private	int[] stage;
	private int countShowCubes = 0;
	private bool gameStart = false;
	private int[] previousStage;
	private bool videoStart  = false;
	private bool [] Stage3State;
	private int currentNumberArduino;
	private bool sphereShow = false;
	private	 int gcount;
	private float[] time;
	private float[] currentTime;



	//private static gameObject current;

	//IList<GameObject>
	// Use this for initialization
	void Start () 
	{
		stage = new int[5];
		Stage3State = new bool[5];
		Stage3State [0] = false;
		Stage3State [1] = false;
		Stage3State [2] = false;
		Stage3State [3] = false;		
		Stage3State [4] = false;
		previousStage = new int[5];
		end = new bool[5];
		end [0] = false;
		end [1] = false;
		end [2] = false;
		end [3] = false;		
		end [4] = false;
		go = new bool[5];
		go [0] = false;
		go [1] = false;
		go [2] = false;
		go [3] = false;		
		go [4] = false;
		value = new int[5];
		time = new float[5];
		currentTime = new float[5];
		button.gameObject.SetActive(false);
		Debug.Log ("enabled");
		comArduino.Open ();
		}

	private void ReadArduino(string s)
	{
		int i = -1;
		//Debug.Log (s);
		while (++i < s.Length) 
		{
			if (s [i] >= '0' && s [i] <= '4')
				currentNumberArduino = s[i] - 48;
		}
		//Debug.Log ("c:" + currentNumberArduino);
		if (s.Contains ("PING"))
		{
			//Debug.Log ("true");
			Stage3State [currentNumberArduino] = true; // it's detached
		}
		if (s.Contains ("PONG"))
		{
			//Debug.Log ("false");
			Stage3State [currentNumberArduino] = false; // 
		}
	}

	public void StartGame()
	{
		//Debug.Log ("here Start");
		if (cubes.Count > 0) 
		{
			parseFile.ParseFileForVideo (cubes);
			gameStart = true;
			button.gameObject.SetActive(true);
		//	Debug.Log ("COucouc");

//			button.enabled = true;
		//	parseFile.ParseFileForVideo ((PlayVideo)(cubes.GetComponent (typeof(PlayVideo))));
		}
	}

	public void ShowVideo()
	{
		videoStart = true;
		foreach (GameObject tmp in cubes)
		{
			GameObject gameObj = ((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube;
			//gameObj.GetComponent<MeshRenderer> ().material. = null;
			PlayVideo playvideo = (PlayVideo)(gameObj.GetComponent(typeof(PlayVideo)));
			if (playvideo.state == PlayVideo.State.Main)
				mainVideo = playvideo;
			MovieInfo movieInfo = playvideo.getTexture(0);
			if (playvideo.state != PlayVideo.State.Main)
				playvideo.PlayVideoTexture(movieInfo.movieTexture, false);
			else
				playvideo.PlayVideoTexture(movieInfo.movieTexture, false);
			gameObj.GetComponent<MeshRenderer> ().material.color = Color.white;
			button.gameObject.SetActive(false);
			/*StartCoroutine
			(
				comArduino.AsynchronousReadFromArduino
				(   ReadArduino,     // Callback
					() => Debug.Log("Error!"), // Error callback
					100f                             // Timeout (seconds)
				)
			);*/
		}

	
		//comArduino.WriteToArduino("")
	}
	// Update is called once per frame
	void Update () 
	{

		//comArduino.ReadFromArduino ();
		if (gameStart == true && videoStart == false) 
		{
			//Debug.Log ("here1");
			int tmpcnt = 0;
			foreach (GameObject tmp in cubes) 
			{
				GameObject gameObj = ((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube;
				if (tmpcnt == countShowCubes)
					gameObj.SetActive (true);
				else
					gameObj.SetActive (false);
				++tmpcnt;
			}
			if (Input.GetKeyDown (KeyCode.N))
				++countShowCubes;
		}

		if (videoStart == true) 
		{
			
			if (gameStart == true) 
			{
				foreach (GameObject tmp in cubes) 
				{
					
					((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube.SetActive (true);

				}
				gameStart = false;
			}
			gcount = 0;

			if (Input.GetKeyDown(KeyCode.C)) 
			{
				sphereShow = !sphereShow;
				foreach (GameObject tmp in cubes) 
				{
					((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube.transform.GetChild (0).gameObject.SetActive (sphereShow);
					((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube.transform.GetChild (1).gameObject.SetActive (sphereShow);
				}
			}


			foreach (GameObject tmp in cubes) 
			{
				currentTime [gcount] += Time.deltaTime; 
				if (videoStart == true) 
				{
					stage [gcount] = 0;
					if (checkStage3 (gcount, tmp) == true)
						stage [gcount] = 3;
					else if (checkStage2 (tmp, gcount) == true)
						stage [gcount] = 2;
					else if (checkStage1 (gcount) == true)
						stage [gcount] = 1;
					if (previousStage [gcount] != stage [gcount]) 
					{
						//Debug.Log ("Handeled");
						HandleStages (stage [gcount], tmp, previousStage[gcount]);
						previousStage [gcount] = stage [gcount];
					}
					specialCase (tmp, stage[gcount]);
				}
				++gcount;
			}
		}
	}

	private void specialCase(GameObject tmp, int currentStage)
	{
		if (mainVideo.gameObject == ((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube && currentStage == 0) 
		{
			if (mainVideo.getTexture (0).movieTexture.isPlaying == false && mainVideo.getTexture (0).extraMovieTexture.isPlaying == false)
				mainVideo.PlayVideoTexture (mainVideo.getTexture (0).extraMovieTexture, true);
		}
	}

	private void HandleStages(int stageBis, GameObject go, int previousStageBis)
	{
		int counter = -1;
		GameObject gameObj;
		while (++counter < NumberOfClips)
		{
			gameObj = ((CollisionCube)go.GetComponent (typeof(CollisionCube))).movableCube;

			if (!(((PlayVideo)gameObj.GetComponent (typeof(PlayVideo))).state == PlayVideo.State.Main && stageBis == 1 && previousStageBis == 2))
				((PlayVideo)gameObj.GetComponent (typeof(PlayVideo))).StopVideoIndex (counter);
			else
				Debug.Log ("YAEAH");
		}
		gameObj = ((CollisionCube)go.GetComponent (typeof(CollisionCube))).movableCube;
		PlayVideo playV = ((PlayVideo)gameObj.GetComponent (typeof(PlayVideo)));
		playV.PlayVideoIndex (stageBis);
		int value = stageBis - previousStageBis;
		MovieInfo movieInfo = playV.getTexture(previousStageBis);
		MovieInfo currentInfo = playV.getTexture(stageBis);
		if (value < 0 && movieInfo.prevMovieTexture != null)
		{
			if (gcount == 1)
				Debug.Log ("Prev: " + stageBis);
			playV.PlayVideoTexture (movieInfo.prevMovieTexture, currentInfo.loop);
		} 
		else if (value > 0 && movieInfo.nextMovieTexture != null) 
		{
			if (gcount == 1)
				Debug.Log ("next: " + stageBis);

			playV.PlayVideoTexture (movieInfo.nextMovieTexture, currentInfo.loop);
		} 
		else if (movieInfo.movieTexture != null)
		{
			if (gcount == 1)
				Debug.Log ("normal: " + stageBis);
			playV.PlayVideoTexture (movieInfo.movieTexture, currentInfo.loop);
		}
		if (value < 0 && movieInfo.prevMainMovieTexture != null) 
		{
			if (gcount == 1)
			Debug.Log ("prevmain: " + stageBis);
			mainVideo.PlayVideoTexture (movieInfo.prevMainMovieTexture, movieInfo.loop);
		} 
		else if (playV.getTexture(stageBis).mainMovieTexture != null) 
		{
			if (gcount == 1)
				Debug.Log ("main: " + stageBis + " " + playV.getTexture(stageBis).mainMovieTexture.name);
			mainVideo.PlayVideoTexture (playV.getTexture(stageBis).mainMovieTexture,
				playV.getTexture(stageBis).mainMovieTexture.loop);
		} 
		else 
		{
			if (gcount == 1)
				Debug.Log ("nothing");
		}
	}

	private bool checkStage1(int count)
	{
		if (gcount == 1)
		Debug.Log ("Stage1 ...");
		uint playerID = KinectManager.Instance.GetPlayer1ID();
		if (cubes.Count > 0 && playerID > 0) 
		{
			int cnt = 0;
			foreach (GameObject tmp in cubes)
			{
				averageCubesDistance += tmp.transform.position;
				++cnt;

			}
			if (end [count] == true) 
			{
				time [count] = currentTime[count] + 1;
				go [count] = false;
			}
			if (gcount == 1)
				Debug.Log("stage1");
			//go [count] = false;
			end [count] = false;
			return (true);
		}
		return (false);
	}

	private bool checkStage2(GameObject tmp, int count)
	{
		if (cubes.Count > 0) 
		{
			CollisionCube changeColor = (CollisionCube)tmp.GetComponent (typeof(CollisionCube));
			if (changeColor.colourChangeCollision == true)
			{	
				//Debug.Log ("stage 2");
				if (gcount == 1)
					Debug.Log (" 2 true");
				if (value [count] % 2 == 0)
					++(value [count]);
				if (Stage3State[count] == false && currentTime[count] > time[count])
					go [count] = true;
				return (true);
			}
			if (go [count] == true && end [count] == false)
				return (true);
		}

			if ((value [count]) % 2 == 1)
				++(value [count]);
			//
			//Stage3State [count] = false;
			//Debug.Log (" 2 false");
			return (false);

	}
		
	private bool checkStage3(int count, GameObject tmp)
	{
		if (Stage3State [count] == true)
			checkStage2 (tmp, count);
		if (go [count] == true && end [count] == false)
		{
			if (gcount == 1)
				Debug.Log ("hello");
			GameObject gameObj = ((CollisionCube)tmp.GetComponent (typeof(CollisionCube))).movableCube;
			PlayVideo playV = ((PlayVideo)gameObj.GetComponent (typeof(PlayVideo)));
			if (playV.getTexture (2).movieTexture != null && playV.getTexture (2).movieTexture.isPlaying == false &&
			    Stage3State [count] == false) {
				//Debug.Log ("Stage 3: TRUE");
				Stage3State [count] = true;
				if ((value[count]) % 2 == 0)
					tmpValue = (value[count]) - 1;
				else
					tmpValue = (value[count]);
				go [count] = false;
				return (true);
			}
		} 
		else if (Stage3State [count] == true && tmpValue + 2 > (value[count])) 
		{
			if (gcount == 1)
			Debug.Log ("TRUE" + (value[count]));
			return (true);
		}

		if (Stage3State [count] == true )
		{
			if (gcount == 1)
				Debug.Log ("FALSE" + (value[count]));
			end [count] = true;
			Stage3State [count] = false;
		}
		return (false);
	}



	public void addCubes(GameObject go)
	{
		cubes.Add (go);
	}


}
