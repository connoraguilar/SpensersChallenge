using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem {

    //public static void SaveSquidNPC(SquidMan npc)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = Application.persistentDataPath + "/Squid.211";
    //    FileStream stream = new FileStream(path, FileMode.Create);
    //   // SquidNPCData squidData = 
    //}
    public static void SaveFiles(GameInformation info, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);
        FileData gameData = new FileData(info);
        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static FileData LoadFiles(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FileData data = formatter.Deserialize(stream) as FileData;
            stream.Close();
            return data;
        }
        else
        {

            return null;
        }
    }

    public static void SaveGame(Chandli player, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);
        GameData gameData = new GameData(player);
        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData LoadGame(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in path " + path);
            return null;
        }
    }

    public static void SaveNPC(NPC npc)
    {
        //BinaryFormatter formatter = new BinaryFormatter();

        //FileStream stream = new FileStream(path, FileMode.Create);
        if(GameInformation.NPCData.Count <= npc.NPCIndex)
        {
            NPCData newNPC = new NPCData();
            for(int i =0; i< npc.NPCIndex+1;i++)
            {
                if(i<= GameInformation.NPCData.Count)
                {
                    GameInformation.NPCData.Insert(i, newNPC);
                }
            }
            //GameInformation.NPCData.Insert(npc.NPCIndex, newNPC);
        }
        GameInformation.NPCData[npc.NPCIndex].hasBeenTalkedTo = npc.HasBeenTalkedTo();
        GameInformation.NPCData[npc.NPCIndex].locked = npc.IsLocked();
        GameInformation.NPCData[npc.NPCIndex].index = npc.NPCIndex;
        GameInformation.NPCData[npc.NPCIndex].tolerance = npc.GetTolerance();

        
        //stream.Close();
    }

    public static void SaveLock(ObjectLock oLock)
    {
        if (GameInformation.ObjectLocks.Count <= oLock.lockIndex)
        {
            LockData newLock = new LockData();
            for (int i = 0; i < oLock.lockIndex + 1; i++)
            {
                if (i <= GameInformation.ObjectLocks.Count)
                {
                    GameInformation.ObjectLocks.Insert(i, newLock);
                }
            }
            //GameInformation.NPCData.Insert(npc.NPCIndex, newNPC);
        }
        GameInformation.ObjectLocks[oLock.lockIndex].locked = oLock.IsLocked();
        GameInformation.ObjectLocks[oLock.lockIndex].index = oLock.lockIndex;
    }

    public static NPCData LoadNPC(string path)
    {

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            NPCData data = formatter.Deserialize(stream) as NPCData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in path " + path);
            return null;
        }
    }

    public static void SavePlayer(Chandli player)
    {

    }

}
