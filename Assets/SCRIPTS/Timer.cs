using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Text timerText;
    private float timerTime;
    private int pastSeconds, pastMinutes; 
    private int setSeconds, setMinutes;

    void Update()
    {
        if(pastMinutes == 2 && pastSeconds == 59)
        {
            //Let Button Appear?
        }
        else
        {
            timerTime += Time.deltaTime;

            pastSeconds = (int)(timerTime % 60);
            pastMinutes = (int)(timerTime / 60) % 60;

            string timerString = string.Format("{0:00}:{1:00}", (setMinutes-pastMinutes), (setSeconds-pastSeconds));

            timerText.text = timerString;
        }
        
    }

    void OnEnable()
    {
        setSeconds = 59;
        setMinutes = 0;
    }
}
