using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite : MonoBehaviour {
    
    private List<string[]> rowData = new List<string[]>();
	//private string[] noteNames = {"C" ,"Cssh" ,"Csh" ,"Df" ,"Dsf" ,"D" ,"Dssh" ,"Dsh" ,"Ef" ,"Esf" ,"E" ,"Ff" ,"Esh" ,"F" ,"Fssh" ,"Fsh" ,"Gf" ,"Gsf" ,"G" ,"Gssh" ,"Gsh" ,"Af" ,"Asf" ,"A" ,"Assh" ,"Ash" ,"Bf" ,"Bsf" ,"B" ,"Cf" ,"Bsh"};
       
    public void Save(string name, string score)
	{
        string[] rowDataTemp;

        // Creating First row of titles manually..
        // rowDataTemp[0] = "Note";
        // rowDataTemp[1] = "Score";
        // rowData.Add(rowDataTemp);

		rowDataTemp = new string[2];
		rowDataTemp[0] = name; // Note name
		rowDataTemp[1] = score; // Score given by the listener.
		rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++)
		{
            output[i] = rowData[i];
        }

        int     length         = output.GetLength(0);
        string     delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath()
	{
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Saved_data.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
}