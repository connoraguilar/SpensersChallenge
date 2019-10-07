using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : Interactable {
    public Image blackScreen;
    public string sceneName;
    private bool isDarkening = false;
    private bool startDarkening = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(startDarkening)
        {
            DarkenScreen();
        }
	}

    public override void Interact()
    {
        startDarkening = true;
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

            color.a += 0.01f;

            blackScreen.color = color;
            if (color.a >= 1)
            {
                isDarkening = false;
            }

        }
        else
        {
            // BrightenScreen(color);
            SceneManager.LoadScene(sceneName);
        }


    }
    
}
