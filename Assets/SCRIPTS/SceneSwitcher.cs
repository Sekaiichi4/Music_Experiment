using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour 
{

	public void SwitchScene(string _name) 
	{
		Debug.Log("PRESSED");
		SceneManager.LoadScene(_name);
	}

	// public void DecideStart(int i)
	// {
	// 	PlayerPrefs.SetInt("MajorOrMinor", i);
	// }
}

	

