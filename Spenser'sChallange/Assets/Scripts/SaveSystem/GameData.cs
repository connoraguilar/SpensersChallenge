using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class GameData  {
   // public string SaveFileName;
    public string CurrentScene;
    public float[] position;
    //NPC Information
    public bool[] locked;
    public bool[] hasBeenTalkedTo;
    public bool[] missionSuccesful;
    public int[] tolerance;
    public int[] index;
    public int numOfNPC;
    public bool[] lockLocked;
    public int[] lockIndex;
    public int numOfLocks;
    public GameData(Chandli player)
    {
        if (player.mFullHealth != -1)
        {
            CurrentScene = SceneManager.GetActiveScene().name;
            position = new float[3];
            position[0] = player.SpawnPosition.x;
            position[1] = player.SpawnPosition.y;
            position[2] = player.SpawnPosition.z;
            numOfNPC = GameInformation.NPCData.Count;
            numOfLocks = GameInformation.ObjectLocks.Count;
            locked = new bool[GameInformation.NPCData.Count];
            hasBeenTalkedTo = new bool[GameInformation.NPCData.Count];
            missionSuccesful = new bool[GameInformation.NPCData.Count];
            tolerance = new int[GameInformation.NPCData.Count];
            index = new int[GameInformation.NPCData.Count];
            lockIndex = new int[GameInformation.ObjectLocks.Count];
            lockLocked = new bool[GameInformation.ObjectLocks.Count];

            for (int i=0; i<GameInformation.NPCData.Count;i++)
            {
                locked[i] = GameInformation.NPCData[i].locked;
                hasBeenTalkedTo[i] = GameInformation.NPCData[i].hasBeenTalkedTo;
                missionSuccesful[i] = GameInformation.NPCData[i].missionSuccesful;
                tolerance[i] = GameInformation.NPCData[i].tolerance;
                index[i] = GameInformation.NPCData[i].index;
            }

            for(int i =0; i< GameInformation.ObjectLocks.Count;i++)
            {
                lockLocked[i] = GameInformation.ObjectLocks[i].locked;
                lockIndex[i] = GameInformation.ObjectLocks[i].index;
            }
        }
        else
        {
            CurrentScene = "Intro";
            position = null;
           
        }
    }
}
