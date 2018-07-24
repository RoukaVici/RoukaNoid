using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoukaViciController : MonoBehaviour
{
	private static RoukaViciController _instance;
	public static RoukaViciController instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<RoukaViciController>();
			}
			return _instance;
		}
	}

	public List<VibrationStyle> vibrationPatterns = new List<VibrationStyle>();
	public List<GameObject> patternButtons = new List<GameObject>();
	public int patternID = 0;

	void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void setVibrationPattern(int newID)
	{
		newID = newID < 0 ? 0 : newID;
		if (newID == patternID)
			return ;
		if (MenuManager.instance != null)
			MenuManager.instance.selectPatternButton(newID, patternID);
		patternID = newID;
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
