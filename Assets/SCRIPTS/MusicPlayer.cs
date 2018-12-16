using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour 
{
	//public List<AudioClip> allAudio;
	public GameObject playingScreen;
	public GameObject playButton;
	public GameObject ratingScreen;
	public Text title;
	public GameObject breakScreen;
	public GameObject finishScreen;
	public bool isPractice, startTiming;
	public int indexPractice, index, hadBreaks;

	public GameObject nextButton;

	private int[] categoryNames = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18};	
	private string[] fileCategories = { "A", "B", "C", "D"};	
	public bool[] categoriesPlayed = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};	
	public Transform buttonQ,buttonX,buttonO;

	private List<Song> currentList = new List<Song>();

	private int randomInt, ListeningOnly;

	private AudioSource audioSource;

	private CsvReadWrite csvWriter;

	private float timerTime, chosenTime;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		InitDatabase();
		index = 0;
		indexPractice = 0;
		hadBreaks = 0;
		ListeningOnly = PlayerPrefs.GetInt("Listening");

		if(ListeningOnly != 1)
		{
			csvWriter = GetComponent<CsvReadWrite>();
			csvWriter.Save("Sound", "Choice", "Time");
		}

		SetTitle();	
		startTiming = false;

		Debug.Log(currentList.Count);
	}

	void Update()
	{
		if(startTiming)
		{
			timerTime += Time.deltaTime;
		}
	}

	public void InitDatabase()
	{
		for(int j = 0; j < 18; j++)
		{
			for(int i = 0; i < 4; i++)
			{
				AudioClip tune;

				if(ListeningOnly != 1)
				{

					tune = (AudioClip) Resources.Load("Quiz/" + (j+1).ToString()
					+ "/" 
					+ fileCategories[i] + (j+1).ToString());
					Song currentSong = new Song(tune, false, tune.name);
					currentList.Add(currentSong);

					Debug.Log("Added " + tune.name);	

				}
				else
				{

					tune = (AudioClip) Resources.Load("Listening/" + (j+1).ToString()
					+ "/" 
					+ fileCategories[i] + (j+1).ToString());
					Song currentSong = new Song(tune, false, tune.name);
					currentList.Add(currentSong);
					
					Debug.Log("Added " + tune.name);	

				}
			}
		}
	}

	public void PlaySong()
	{
		// if(isPractice)
		// {
		// 	playButton.SetActive(false);
		// 	AudioClip currentTune = currentList[randomInt].audio; 	//Get audio from list at random 

		// 	if(ListeningOnly != 1)
		// 	{
		// 		Invoke("GetRatingScreen", currentTune.length);
		// 	}
		// 	else
		// 	{
		// 		Invoke("SendRating", currentTune.length);
		// 	}
			
		// 	audioSource.PlayOneShot(currentTune); 
		// }
		// else
		// {
			randomInt = Random.Range(0, currentList.Count);
			string checkName = currentList[randomInt].name.Remove(0,1);
			int checkNumber = System.Int32.Parse(checkName)-1;
			Debug.Log(checkNumber);
			
			while(currentList[randomInt].played || categoriesPlayed[checkNumber])
			{
				randomInt = Random.Range(0, currentList.Count);
				checkName = currentList[randomInt].name.Remove(0,1);
				checkNumber = System.Int32.Parse(checkName)-1;
				Debug.Log(checkNumber);
			}

			categoriesPlayed[checkNumber] = true;

			playButton.SetActive(false);
			AudioClip currentTune = currentList[randomInt].audio; 	//Get audio from list at random 
			currentList[randomInt].played = true;					//and enable the bool

			if(ListeningOnly != 1)
			{
				Invoke("GetRatingScreen", currentTune.length);
			}
			else
			{
				Invoke("SendRating", currentTune.length);
			}
			
			audioSource.PlayOneShot(currentTune); 
		// }
	}

	public void SetChoice (string _choice) 
	{
		PlayerPrefs.SetString("Choice", _choice);
		nextButton.SetActive(true);
		chosenTime = (float) System.Math.Round(timerTime, 3);
	}

	public void SendRating () 
	{
		if(isPractice)
		{
			if(indexPractice == 2)
			{
				isPractice = false;
			}
			indexPractice++;
			GetPlayingScreen();
		}
		else
		{
			if(ListeningOnly != 1)
			{
				string _rating = PlayerPrefs.GetString("Choice");
				Debug.Log(_rating + " " + chosenTime);

				csvWriter.Save(currentList[randomInt].name, _rating, chosenTime.ToString("F3"));
			}
			index++;

			if(index == 5) 
			{
				GetBreakScreen();
				ShuffleButtons();
			}
			else if(index == 10) 
			{
				GetFinishScreen();
			}
			else if(index == 15) 
			{
				GetFinishScreen();
			}
			else
			{
				GetPlayingScreen();
			}
		}

		nextButton.SetActive(false);
	}

	public void EndBreak()
	{
		breakScreen.SetActive(false);
		GetPlayingScreen();
		hadBreaks++;
	}

	void SetTitle()
	{
		if(isPractice)
		{
			title.text = "Practice " + (indexPractice+1).ToString() + "/3";
		}
		else
		{
			title.text = (index+1).ToString() + "/15";
		}
	}

	void GetRatingScreen()
	{
		playingScreen.SetActive(false);
		ratingScreen.SetActive(true);
		timerTime = 0;
		startTiming = true;
	}

	void GetPlayingScreen()
	{
		playButton.SetActive(true);
		NextMelody();
		ratingScreen.SetActive(false);
		SetTitle();
		playingScreen.SetActive(true);
	}

	void GetBreakScreen()
	{
		playingScreen.SetActive(false);
		ratingScreen.SetActive(false);
		breakScreen.SetActive(true);
	}

	void GetFinishScreen()
	{
		playingScreen.SetActive(false);
		ratingScreen.SetActive(false);
		breakScreen.SetActive(false);
		finishScreen.SetActive(true);
	}

	void NextMelody()
	{
		randomInt = Random.Range(0, currentList.Count);
	}

	void ShuffleButtons()
	{
		Vector3 xPos = buttonX.position;
		Vector3 oPos = buttonO.position;
		Vector3 qPos = buttonQ.position;

		buttonQ.position = xPos;
		buttonX.position = oPos;
		buttonO.position = qPos;
	}

}
