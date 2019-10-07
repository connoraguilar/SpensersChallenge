using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveFileButton : MonoBehaviour {
    public Button button;
    public Text buttonText;
    public GameInformation info;
    private GameData data;
    public int index;
    private bool changedText = true;
    private string scene;
    private string path;
	// Use this for initialization
	void Start () {

        
        if (info.currentNumberOfFiles >index)
        {
            changedText = true;
            buttonText.text = info.saveFiles[index];
            path = Application.persistentDataPath + "/" + info.saveFiles[index] + ".211";
            data = SaveSystem.LoadGame(path);
            NPCDefine();
            LockDefine();
            scene = data.CurrentScene;
        }
        else
        {
            changedText = false;
        }
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
        if (!changedText)
        {
            if (info.currentNumberOfFiles > index)
            {
                changedText = true;
                buttonText.text = info.saveFiles[index];
                path = Application.persistentDataPath + "/"+ info.saveFiles[index] + ".211";
                data = SaveSystem.LoadGame(path);
                NPCDefine();
                LockDefine();
                scene = data.CurrentScene;
            }
        }
	}

    void TaskOnClick()
    {
        //GameInformation.playerLoadPos = new Vector3(data.position[0], data.position[1], data.position[2]);
        info.currentSaveFile = buttonText.text;
        GameInformation.playerPath += "/" + buttonText.text + ".211";
        GameInformation.levelLoadedIn = scene;
        SceneManager.LoadScene(scene);
    }

    public void SetScene(string scene)
    {
        this.scene = scene;
    }

    void NPCDefine()
    {
        GameInformation.NPCData = new List<NPCData>();
        for(int i =0; i< data.numOfNPC;i++)
        {
            NPCData newNPC = new NPCData();
            newNPC.hasBeenTalkedTo = data.hasBeenTalkedTo[i];
            newNPC.locked = data.locked[i];
            newNPC.missionSuccesful = data.missionSuccesful[i];
            newNPC.index = data.index[i];
            newNPC.tolerance = data.tolerance[i];
            GameInformation.NPCData.Insert(i, newNPC);
        }
    }

    void LockDefine()
    {
        GameInformation.ObjectLocks = new List<LockData>();
        for (int i = 0; i < data.numOfLocks; i++)
        {
            LockData newLock = new LockData();
            newLock.locked = data.lockLocked[i];
            newLock.index = data.lockIndex[i];
            GameInformation.ObjectLocks.Insert(i, newLock);
        }
    }
}
