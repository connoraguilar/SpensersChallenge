using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public Canvas pauseMenu;
    public EventSystem eventSystem;
    private bool isPaused = false;
    private bool unPause = false;
    private Animator[] animators;
    private MonoBehaviour[] scripts;
    // Use this for initialization
    void Start () {
        UnityEngine.Object[] objs = FindObjectsOfType(typeof(Animator));
        animators = Array.ConvertAll(objs, item => item as Animator);
        objs = FindObjectsOfType(typeof(MonoBehaviour));
        scripts = Array.ConvertAll(objs, item => item as MonoBehaviour);

       // Component[] components = pauseMenu.GetComponentsInChildren<MonoBehaviour>();
        //for (int j = 0; j < pauseMenu.gameObject.transform.childCount; j++)
        //{
        //    pauseMenu.gameObject.transform.GetChild(j).gameObject.SetActive(false);
            
            
        //}
        pauseMenu.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		if((Input.GetKeyDown(KeyCode.Escape) && !isPaused))
        {
           
            isPaused = true;
          


            GameInformation.isPaused = true;
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].enabled = false;
            }
            for(int i=0; i<scripts.Length;i++)
            {
                if(scripts[i] != this && scripts[i] != eventSystem && scripts[i].GetComponentsInParent<Exclude>().Length ==0)
                {
                    scripts[i].enabled = false;
                }
            }
            //for (int j = 0; j < pauseMenu.gameObject.transform.childCount; j++)
            //{
            //    pauseMenu.gameObject.transform.GetChild(j).gameObject.SetActive(true);
            //}
            pauseMenu.gameObject.SetActive(true);

        }
        else if((Input.GetKeyDown(KeyCode.Escape) && isPaused) || unPause)
        {
            unPause = false;
            isPaused = false;
           
            GameInformation.isPaused = false ;
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].enabled = true;
            }
            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i] != this)
                {
                    scripts[i].enabled = true;
                }
            }

            //for (int j = 0; j < pauseMenu.gameObject.transform.childCount; j++)
            //{
            //    pauseMenu.gameObject.transform.GetChild(j).gameObject.SetActive(false);
            //}
            pauseMenu.gameObject.SetActive(false);
        }
	}

    public void UnPause()
    {
        unPause = true;
    }
    
}
