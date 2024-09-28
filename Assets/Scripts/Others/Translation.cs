using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{   
    public string PressButton;
    public string Level;
    public string Victory;
    public string Continue;
    public string GameLose;
    public string OpenCellForRewarded;
    public string NoPlace;
    public string Repeat;

    public string BackerHint;
    public string TriplerHint;
    public string ResorterHint;

    public string Hint0Level;



    public Translation() { }
}
