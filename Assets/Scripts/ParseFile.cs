using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ParseFile : MonoBehaviour
{

	public void ParseFileForVideo(List<GameObject> list)
	{
		StreamReader reader = File.OpenText("videoName.txt");
		string line;
		int cnt = 0;
		int state = 0;
		GameObject gameObj = ((CollisionCube)list [cnt].GetComponent (typeof(CollisionCube))).movableCube;
		PlayVideo currentPlayVideo = (PlayVideo)(gameObj.GetComponent(typeof(PlayVideo)));
		while ((line = reader.ReadLine()) != null) 
		{
			if (line [0] != '-') 
			{
				line.Replace (@"\", "");
				if (line.Contains ("prev:")) {
					currentPlayVideo.setPrevVideoName (line.Substring (5));
				//	Debug.Log ("Prev:" + line.Substring (5));
				} else if (line.Contains ("next:")) {
					currentPlayVideo.setNextVideoName (line.Substring (5));
				//	Debug.Log ("next:" + line.Substring (5));


				}
				else if (line.Contains ("prevmain:")) {
					//string str = line.Substring (9);
					currentPlayVideo.setPrevMainVideoName (line.Substring (9));
				//	Debug.Log ("prevmain:" + line.Substring (9));
				}

				else if (line.Contains ("main:")) {
					//string str = line.Substring (5);
					currentPlayVideo.setMainVideoName (line.Substring (5));
				//	Debug.Log ("main:" + line.Substring (5));

				}
				else if (line.Contains ("extra:")) 
				{
				//	string str = line.Substring (6);
					currentPlayVideo.setExtraVideoName(line.Substring (6));
				//	Debug.Log ("main:" + line.Substring (5));
				}
				else if (line.Contains ("loop")) 
				{
					currentPlayVideo.setLoopt ();
				//	Debug.Log ("loop");
				}
				else 
				{
					//string str = line + 5;
				//	Debug.Log ("default:" + (line + 5));
					currentPlayVideo.setVideoName (line, state);
					++state;
				}
				//Debug.Log ("filename: " + line);

			} 
			else 
			{
				//Debug.Log("new");
				++cnt;
				if (list.Count <= cnt)
					return ;
				state = 0;
				gameObj = ((CollisionCube)list [cnt].GetComponent (typeof(CollisionCube))).movableCube;
				currentPlayVideo = (PlayVideo)(gameObj.GetComponent(typeof(PlayVideo)));
				//A a[0] = list.ToArray ();
			}
		}
	}

}
