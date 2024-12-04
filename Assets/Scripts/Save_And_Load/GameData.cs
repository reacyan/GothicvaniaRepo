using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int currency;
    public string rebornPointId;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, bool> checkPoint;
    public SerializableDictionary<string, int> inventory;

    public List<string> equipmentId;

    public GameData()
    {
        this.currency = 0;

        rebornPointId=string.Empty;
        skillTree = new SerializableDictionary<string, bool>();
        checkPoint= new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
    }
}
