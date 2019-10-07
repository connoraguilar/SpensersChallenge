using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public void Load(string saveFile)
    {
        GameData data = SaveSystem.LoadGame(Application.persistentDataPath + "/" + saveFile + ".211");
        if (data != null)
        {
            if (data.position != null)
            {
                GameInformation.playerLoadPos = new Vector3(data.position[0], data.position[1], data.position[3]);
            }
        }
    }
}