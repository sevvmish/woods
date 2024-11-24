using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{   
    public string W;
    public string A;
    public string S;
    public string D;

    public string Attack;
    public string Block;
    public string Jump;

    public string Collect;
    public string Chop;
    public string Mine;
    public string E;

    [Header("Assets")]
    public string TreeSmall;
    public string TreeHard;



    public Translation() { }
}
