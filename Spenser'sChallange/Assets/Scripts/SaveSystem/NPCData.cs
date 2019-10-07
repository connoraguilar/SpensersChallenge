using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCData {
    public bool locked;
    public bool hasBeenTalkedTo;
    public bool missionSuccesful;
    public int tolerance;
    public int index;
    //public NPCData(NPC character)
    //{
    //    hasBeenTalkedTo = character.HasBeenTalkedTo();
    //    locked = character.IsLocked();
    //    tolerance = character.GetTolerance();
    //}
}
