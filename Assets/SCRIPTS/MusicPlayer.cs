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

	private string[][] quizLists = new string[][] 	
										{	new string[] {"B3","C4","D6","E1","A5","F2"},
											new string[] {"E4","C3","A2","D5","B1","F6"},
											new string[] {"B6","F3","A4","D2","E5","C1"},
											new string[] {"B2","C5","D3","A1","F4","E6"},
											new string[] {"E3","C2","F1","D4","A6","B5"},
											new string[] {"C6","B4","D1","A3","F5","E2"}
										};

	private string[][] listeningLists = new string[][] 
										{	new string[] {"A3","C5","B6","F4","E1","D2"},
											new string[] {"E6","B3","F2","A5","C4","D1"},
											new string[] {"B1","A6","C2","D4","E5","F3"},
											new string[] {"E4","A1","F6","B2","D5","C3"},
											new string[] {"D3","C6","F1","B5","E2","A4"},
											new string[] {"D6","A2","C1","F5","E3","B4"}
								 		};
	private int[] categoryNames = { 1, 2, 3, 4, 5, 6};	
	private string[] fileCategories = { "A", "B", "C", "D", "E", "F"};	
	//public bool[] categoriesPlayed = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};	
	public Transform buttonQ,buttonX,buttonO;

	private List<Song> allSongs = new List<Song>();
	private List<Song> currentList = new List<Song>();

	private int participantNumber, ListeningOnly;

	private AudioSource audioSource;

	private CsvReadWrite csvWriter;

	private float timerTime, chosenTime;

	void Start()
	{
		participantNumber = PlayerPrefs.GetInt("Participant");
		audioSource = GetComponent<AudioSource>();
		InitDatabase();
		index = 0;
		indexPractice = 0;
		hadBreaks = 0;
		ListeningOnly = PlayerPrefs.GetInt("Listening");

		if(ListeningOnly != 1)
		{
			csvWriter = GetComponent<CsvReadWrite>();
			csvWriter.Save("Participant", "=", "participantNumber");
			csvWriter.Save("Sound", "Choice", "Time");
		}

		SetTitle();	
		startTiming = false;

		Debug.Log(allSongs.Count);
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
		//FIRST 2 PRACTICE SOUNDS
		AudioClip p1 = (AudioClip) Resources.Load("Practice/P1");
		Song p1Song = new Song(p1, false, p1.name);
		currentList.Add(p1Song);
		AudioClip p2 = (AudioClip) Resources.Load("Practice/P2");
		Song p2Song = new Song(p1, false, p1.name);
		currentList.Add(p2Song);

		//LOAD ALL REAL SOUNDS
		for(int j = 0; j < 6; j++)
		{
			for(int i = 0; i < 6; i++)
			{
				AudioClip tune;

				if(ListeningOnly != 1)
				{

					tune = (AudioClip) Resources.Load("Sounds/" + (j+1).ToString()
					+ "/" 
					+ fileCategories[i] + (j+1).ToString());
					Song currentSong = new Song(tune, false, tune.name);
					allSongs.Add(currentSong);

					Debug.Log("Added " + tune.name);	

				}
				else
				{

					tune = (AudioClip) Resources.Load("Listening/" + (j+1).ToString()
					+ "/" 
					+ fileCategories[i] + (j+1).ToString());
					Song currentSong = new Song(tune, false, tune.name);
					allSongs.Add(currentSong);
					
					Debug.Log("Added " + tune.name);	

				}
			}
		}

		//MAKE LIST FOR CURRENT PARTICIPANT
		for(int h = 0; h < 6; h++)
		{
			if(ListeningOnly != 1)
			{
				Song currentSong = allSongs.Find(x => x.name == quizLists[(participantNumber%6)-1][h]);
				currentList.Add(currentSong);
			}
			else
			{
				Song currentSong = allSongs.Find(x => x.name == listeningLists[(participantNumber%6)-1][h]);
				currentList.Add(currentSong);
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
			// randomInt = Random.Range(0, allSongs.Count);
			// string checkName = allSongs[randomInt].name.Remove(0,1);
			// int checkNumber = System.Int32.Parse(checkName)-1;
			// Debug.Log(checkNumber);
			
			// while(allSongs[randomInt].played || categoriesPlayed[checkNumber])
			// {
			// 	randomInt = Random.Range(0, allSongs.Count);
			// 	checkName = allSongs[randomInt].name.Remove(0,1);
			// 	checkNumber = System.Int32.Parse(checkName)-1;
			// 	Debug.Log(checkNumber);
			// }

			// categoriesPlayed[checkNumber] = true;

			playButton.SetActive(false);
			AudioClip currentTune = currentList[index].audio; 
			//allSongs[randomInt].played = true;					

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
			if(indexPractice == 1)
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

				csvWriter.Save(currentList[index].name, _rating, chosenTime.ToString("F3"));
			}
			index++;

			// if(index == 3) 
			// {
			// 	GetBreakScreen();
			// 	ShuffleButtons();
			// }
			if(index == 6) 
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
			title.text = "Practice " + (indexPractice+1).ToString() + "/2";
		}
		else
		{
			title.text = (index+1).ToString() + "/6";
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
		// NextMelody();
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

	// void NextMelody()
	// {
	// 	//randomInt = Random.Range(0, currentList.Count);
	// }

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
