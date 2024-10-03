using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{   
    public string L;
    public int M;
    public int S;
    public int Mus;

    public bool IsLowFPS;    
    public bool AdvOff;

    public int Seed;
    public System.Random MainRandom;

    

    public float Zoom;
    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        
        Zoom = 40;

        IsLowFPS = false;        
        AdvOff = false;

        Seed = 157;
        MainRandom = new System.Random(Seed);

        Debug.Log("created PlayerData instance");
    }


}
