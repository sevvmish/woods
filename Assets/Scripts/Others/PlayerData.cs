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

    public int TerrainSeed;
    public int NatureSeed;

    public float T;
    public int D;

    public int[,] Inv;
    public int[] Equip;
    public int[,] Dur;


    public float Zoom;
    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        
        Zoom = 55;

        T = 0; //time
        D = 0; //day

        IsLowFPS = false;        
        AdvOff = false;

        Seed = 157;
        MainRandom = new System.Random(Seed);

        TerrainSeed = 22;
        NatureSeed = 199;

        Inv = new int[32, 2]; //Inventory ID and Amount
        Equip = new int[0]; //what index in Inventory is equiped on player
        Dur = new int[0,0]; //current durability of a specific items

        Debug.Log("created PlayerData instance");
    }


}
