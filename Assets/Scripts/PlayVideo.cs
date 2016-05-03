using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayVideo : MonoBehaviour 
{

	public enum State{Main, Car};
	public State state = State.Car;
	public MeshRenderer mr;
	public Camera cam;
	static int counterStatic;
	static GameObject main;

	//public string videoName;

	public List<MovieTexture> movieTex;
	public List<MovieInfo> movieInfo;
	MovieInfo currentMovieInfo;

	void Start()
	{
		this.gameObject.transform.LookAt (cam.transform);
	}
		
	public void setVideoName(string name, int state)
	{
		GameObject go = (GameObject)Instantiate (Resources.Load ("MovieTextureInfo"));
		currentMovieInfo =  ((MovieInfo)go.GetComponent(typeof(MovieInfo)));
		movieInfo.Add (currentMovieInfo);
		currentMovieInfo.State = state;
		currentMovieInfo.movieTexture = Resources.Load (name) as MovieTexture;
	}

	public void setLoopt()
	{
		Debug.Log ("Did it sir" + currentMovieInfo.State);
		currentMovieInfo.loop = true;
	}

	public void setPrevVideoName(string name)
	{
		currentMovieInfo.prevMovieTexture = Resources.Load (name) as MovieTexture;
	}

	public void setNextVideoName(string name)
	{
		currentMovieInfo.nextMovieTexture = Resources.Load (name) as MovieTexture;
	}

	public void setMainVideoName(string name)
	{
		currentMovieInfo.mainMovieTexture = Resources.Load (name) as MovieTexture;
	}

	public void setExtraVideoName(string name)
	{
		currentMovieInfo.extraMovieTexture = Resources.Load (name) as MovieTexture;
	}
	public void setPrevMainVideoName(string name)
	{
		currentMovieInfo.prevMainMovieTexture = Resources.Load (name) as MovieTexture;
	}


	public MovieInfo getTexture(int state)
	{
		return (movieInfo [state]);
	}	

	public void PlayVideoTexture(MovieTexture mt, bool isloop)
	{
		if (mt != null) 
		{
			mr.material.mainTexture = mt;
			mt.loop = isloop;
			mt.Play ();
		}
	}
		
	public void PlayVideoIndex(int a)
	{
		int cnt = 0;
		foreach (MovieTexture tmp in movieTex)
		{					
			if (cnt == a) 
			{
				mr.material.mainTexture = tmp;
				tmp.Play ();
			}
			++cnt;
		}
	}
		
	public void StopVideoIndex(int a)
	{
		int cnt = 0;
		foreach (MovieInfo tmp in movieInfo) 
		{
			if (cnt == a) 
			{
				if (tmp.movieTexture != null)
					tmp.movieTexture.Stop ();
				if (tmp.nextMovieTexture != null)
					tmp.nextMovieTexture.Stop ();
				if (tmp.prevMovieTexture != null)
					tmp.prevMovieTexture.Stop ();
				if (tmp.extraMovieTexture != null)
					tmp.extraMovieTexture.Stop ();		
				if (tmp.prevMainMovieTexture != null && tmp.prevMainMovieTexture.isPlaying == false)
					tmp.prevMainMovieTexture.Stop ();
				if (tmp.mainMovieTexture != null)
					tmp.mainMovieTexture.Stop ();
				//tmp.Stop ();
			}
			++cnt;
		}
	}
}
