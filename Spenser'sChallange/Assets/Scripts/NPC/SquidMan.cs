using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SquidMan : NPC {
    public Image blackScreen;
    public TextAsset completedMinigame;
    public TextAsset failedMinigame;
    private bool isDarkening = false;
    // Use this for initialization
    public override void Start () {
        tolerance = 1;
        
        if(GameInformation.inMinigame)
        {
            GameInformation.inMinigame = false;
            int replace  =mDialouge.Count - 3;
            this.hasBeenTalkedTo = true;
            if (GameInformation.failedMinigame)
            {
                mDialouge[replace] = failedMinigame;
            }
            else
            {
                mDialouge[replace] = completedMinigame;
            }
            SaveSystem.SaveNPC(this);
        }

        base.Start();

    }
	
	// Update is called once per frame
	public override void Update () {
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
        GameInformation.inMinigame = true;
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
            SceneManager.LoadScene("ArmWrestle");
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
