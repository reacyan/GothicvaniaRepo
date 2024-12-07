using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class GameData
{
    public int currency;
    public int Lossing;
    public string rebornPointId;
    public Vector2 PlayerDiePosition;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, bool> checkPoint;
    public SerializableDictionary<string, int> inventory;

    public List<string> equipmentId;

    public GameData()
    {
        this.currency = 0;
        this.Lossing = 0;

        rebornPointId=string.Empty;
        skillTree = new SerializableDictionary<string, bool>();
        checkPoint= new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
    }
}
