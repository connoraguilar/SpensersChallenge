using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PTAMom : NPC
{
    public Image blackScreen;
    private bool isDarkening = false;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        tolerance = 2;

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!GameInformation.isPaused)
        {
            base.Update();
            if (finishedReading && !locked)
            {
                DarkenScreen();
            }
        }
    }

    public override void ReadDialouge()
    {
        base.ReadDialouge();

    }

    void DarkenScreen()
    {
        Color color = blackScreen.color;
        if (!isDarkening && color.a == 0)
        {
            isDarkening = true;
        }

        if (isDarkening)
        {

            color.a += 0.005f;

            blackScreen.color = color;
            if (color.a >= 1)
            {
                isDarkening = false;
            }

        }
        else
        {
            SceneManager.LoadScene("Driving");
        }


    }

    //    void BrightenScreen(Color color)
    //    {
    //        color.a -= 0.005f;
    //        if (color.a <= 0.005)
    //        {
    //            color.a = 0;
    //            finishedReading = false;
    //        }
    //        blackScreen.color = color;
    //    }
}
