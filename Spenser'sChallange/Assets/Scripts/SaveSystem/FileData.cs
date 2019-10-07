using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileData {

    public string[] saveFiles;
    public int cap;
    public FileData(GameInformation files)
    {
        cap = files.saveFiles.Count;
        saveFiles = new string[cap];
        for(int i =0; i<cap;i++)
        {
            saveFiles[i] = files.saveFiles[i];
        }

    }
}
