using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameplayUI : MonoBehaviour
{    
    [SerializeField] private PCHelper pcHelper;
    [SerializeField] private GameObject mobileButtons;


    // Start is called before the first frame update
    void Start()
    {
        if (Globals.IsMobile)
        {
            Destroy(pcHelper.gameObject);
            mobileButtons.SetActive(true);
        }
        else
        {
            pcHelper.gameObject.SetActive(true);
            Destroy(mobileButtons);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
