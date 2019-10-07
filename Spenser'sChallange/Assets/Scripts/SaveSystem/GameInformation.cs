using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour {
    public List<string> saveFiles;
    public static List<NPCData> NPCData;
    public static List<LockData> ObjectLocks;
    public static Chandli player;
    public static Vector3 playerLoadPos;
    public static string levelLoadedIn;
    public string currentSaveFile;
    private static string path;
    public static string playerPath;
    public static bool isPaused = false;
    public static bool inMinigame = false;
    public static bool failedMinigame = false;
    public int currentNumberOfFiles = 0;
    void Start()
    {
        path = Application.persistentDataPath + "/SaveFiles.211";
        playerPath = Application.persistentDataPath;
        FileData data = SaveSystem.LoadFiles(path);
        if (data != null)
        {
            currentNumberOfFiles = data.cap;
            for (int i = 0; i < data.cap; i++)
            {
                saveFiles.Add(data.saveFiles[i]);
            }
        }
    }

    public void CreateNewSaveFile(string fileName)
    {
        saveFiles.Insert(currentNumberOfFiles, fileName);
       int i = saveFiles.Capacity;
        SaveSystem.SaveFiles(this, path);
        player.mFullHealth = -1;
        SaveSystem.SaveGame(player, Application.persistentDataPath + "/"+ fileName +".211");
        currentNumberOfFiles++;
        
    }

    public string GetPlayerPath()
    {
        return playerPath;
    }
}
