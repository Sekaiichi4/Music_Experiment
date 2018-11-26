using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song 
{
	public AudioClip audio;
	public bool played;
	public string name;

    // Use this for initialization
    public Song(AudioClip audio, bool played, string name)
    {
		this.audio = audio;
		this.played = played;
		this.name = name;
    }

}
