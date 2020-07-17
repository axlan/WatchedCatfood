using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StopWatch : MonoBehaviour
{

    private float time;
    bool running;


    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            running = !running;
        }
        if (running)
        {
            time += Time.deltaTime;
            GameAssets.i.timerText.text = String.Format("{0:00.00}", time);
        }
    }
}
