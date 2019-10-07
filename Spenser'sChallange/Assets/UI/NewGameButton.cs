using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewGameButton : MonoBehaviour {
   public Button button;
    public GameInformation info;
    public GameObject input;
	// Use this for initialization
	void Start () {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        input.SetActive(false);
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        if (info.currentNumberOfFiles < 3)
        {
            input.SetActive(true);
        }
    }
    public void HideAll()
    {
        input.SetActive(false);
    }
}
