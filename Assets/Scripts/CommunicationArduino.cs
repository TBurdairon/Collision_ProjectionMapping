using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;

public class CommunicationArduino : MonoBehaviour 
{


	/* The serial port where the Arduino is connected. */
	[Tooltip("The serial port where the Arduino is connected")]
	public string port = "\\\\.\\COM10";
	/* The baudrate of the serial port. */
	[Tooltip("The baudrate of the serial port")]
	public int baudrate = 9600;

	private SerialPort stream;

	public void Open () {
		// Opens the serial port
		/*stream = new SerialPort(port, baudrate);
		stream.ReadTimeout = 50;
		stream.Open ();

		if (stream.IsOpen == true)
			Debug.Log ("open");
		else
			Debug.Log ("closed");*/
		//this.stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
	}

	public void WriteToArduino(string message)
	{
		// Send the request
		stream.WriteLine(message);
		stream.BaseStream.Flush();
	}

	public string ReadFromArduino(int timeout = 0)
	{
		stream.ReadTimeout = timeout;
		try
		{
			return stream.ReadLine();
		}
		catch (TimeoutException)
		{
			return null;
		}
	}


	public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
	{
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);

		string dataString = null;

		do
		{
			// A single read attempt
			try
			{
				dataString = stream.ReadLine();
			}
			catch (TimeoutException)
			{
				dataString = null;
			}

			if (dataString != null)
			{
				callback(dataString);
				yield return null;
			} else
				yield return new WaitForSeconds(0.75f);

			nowTime = DateTime.Now;
			diff = nowTime - initialTime;
			//Debug.Log("hello" + diff.Seconds + " | " + timeout);
		} while (diff.Seconds < timeout);

		if (fail != null)
			fail();
		yield return null;
	}

	public void Close()
	{
		stream.Close();
	}

}
