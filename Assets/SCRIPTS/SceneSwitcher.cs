using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour 
{
	public GameObject StartPanel, QuizPanel, ListeningPanel, NextButton;

	public void SwitchScene(string _name) 
	{
		Debug.Log("PRESSED");
		SceneManager.LoadScene(_name);
	}

	public void DecideStart(int i)
	{
		PlayerPrefs.SetInt("Listening", i);
	}

	public void DecideParticipant(int i)
	{
		PlayerPrefs.SetInt("Participant", i);
	}

	public void QuitApplication()
	{
		Application.Quit();
	}

	public void ShowQuizPanel()
	{
		StartPanel.SetActive(false);
		QuizPanel.SetActive(true);
	}

	public void ShowListeningPanel()
	{
		StartPanel.SetActive(false);
		ListeningPanel.SetActive(true);
	}
	
	public void ShowNextButton()
	{
		NextButton.SetActive(true);
	}
}

	

