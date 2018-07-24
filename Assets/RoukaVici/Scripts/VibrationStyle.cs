using UnityEngine;
using System.Collections;
using System.IO;

public class VibrationStyle
{

	public string name;
	public float delay;
	public Fingers[] fingers = new Fingers[10];

	public string getName()
	{
		return this.name;
	}

	public float getDelay()
	{
		return this.delay;
	}

	public Fingers[] getFingers()
	{
		return this.fingers;
	}

	public Fingers getFingerById(int id)
	{
		Debug.Log (this.fingers);
		return this.fingers [id];
	}

	public void addFinger(Fingers fing)
	{
		this.fingers [fing.id] = new Fingers();
		this.fingers [fing.id].id = fing.id;
		this.fingers [fing.id].pattern = fing.pattern;
	}
}
