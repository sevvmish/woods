using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameManager : MonoBehaviour
{
    [Inject] private Musics musics;

    private void Awake()
    {
        Globals.SetQualityLevel();

    }

    private void Start()
    {
        musics.StartMusic();        
    }


    /*
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();
        }
    }
    */

}
