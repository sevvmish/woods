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
    public int Level;
    public int Gold;
    public int Backer;
    public int Trippler;
    public int Resorter;

    public float Zoom;
    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        
        Level = 0;
        Gold = 0;

        Zoom = 50;

        IsLowFPS = false;        
        AdvOff = false;

        Backer = 0;
        Trippler = 0;
        Resorter = 0;

        Debug.Log("created PlayerData instance");
    }


}
