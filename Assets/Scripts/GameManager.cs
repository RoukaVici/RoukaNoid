using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private CursorLockMode mode;
	// Use this for initialization
	void Start ()
	{
		Cursor.lockState = mode;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetAxis("Cancel") > 0)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus == true)
		{
			Cursor.lockState = mode;
		}
	}

	public void resetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
