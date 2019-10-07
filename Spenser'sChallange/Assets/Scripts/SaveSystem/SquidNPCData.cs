using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidNPCData {
    public bool locked;
    public bool hasBeenTalkedTo;
    public bool missionSuccesful;
    public int tolerance;

    public SquidNPCData(SquidMan character)
    {
        hasBeenTalkedTo = character.HasBeenTalkedTo();
        locked = character.IsLocked();
        tolerance = character.GetTolerance();
    }
}
