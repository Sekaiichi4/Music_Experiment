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

	private string[] categoryNames = { "A", "B", "C", "D"};	
	
	private List<Song> currentList = new List<Song>();

	private int randomInt;

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

		csvWriter = GetComponent<CsvReadWrite>();
		csvWriter.Save("Sound", "Choice", "Time");
		SetTitle();	
		startTiming = false;

		Debug.Log(currentList.Count);

		//todo: Let CSVWRITER print the first line through a method or something.
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
		for(int j = 0; j < 4; j++)
		{
			for(int i = 1; i <= 18; i++)
			{
				AudioClip tune;

				tune = (AudioClip) Resources.Load("Sounds/" + categoryNames
				[j] + "/" + categoryNames
				[j] +i);
				Song currentSong = new Song(tune, false, tune.name);
				currentList.Add(currentSong);
				Debug.Log("Added " + tune.name);	
			}
		}
	}

	public void PlaySong()
	{
		if(isPractice)
		{
			playButton.SetActive(false);
			AudioClip currentTune = currentList[randomInt].audio; 	//Get audio from list at random 

			Invoke("GetRatingScreen", currentTune.length);
			
			audioSource.PlayOneShot(currentTune); 
		}
		else
		{
			randomInt = Random.Range(0, currentList.Count);
			
			while(currentList[randomInt].played)
			{
				randomInt = Random.Range(0, currentList.Count);
			}

			playButton.SetActive(false);
			AudioClip currentTune = currentList[randomInt].audio; 	//Get audio from list at random 
			currentList[randomInt].played = true;					//and enable the bool

			Invoke("GetRatingScreen", currentTune.length);
			
			audioSource.PlayOneShot(currentTune); 
		}
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
			string _rating = PlayerPrefs.GetString("Choice");
			Debug.Log(_rating + " " + chosenTime);

			csvWriter.Save(currentList[randomInt].name, _rating, chosenTime.ToString("F3"));
			index++;

			if(index == 5 || index == 10) 
			{
				GetBreakScreen();
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
			title.text = "練習 " + (indexPractice+1).ToString() + "/3";
		}
		else
		{
			title.text = "本試行 " + (index+1).ToString() + "/15";
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

}
