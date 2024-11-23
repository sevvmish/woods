using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    float _timer;


    private void Update()
    {
        if (_timer > 1)
        {
            _timer = 0;
            print("TICK");
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }




}
