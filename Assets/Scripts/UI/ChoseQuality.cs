using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoseQuality : MonoBehaviour
{
    [SerializeField] private Button goodQ;
    [SerializeField] private Button badQ;

    // Start is called before the first frame update
    void Start()
    {
        goodQ.onClick.AddListener(() => 
        {
            Globals.IsLowFPS = false;
            SceneManager.LoadScene("Gameplay");
        });

        badQ.onClick.AddListener(() =>
        {
            Globals.IsLowFPS = true;
            SceneManager.LoadScene("Gameplay");
        });
    }

}
