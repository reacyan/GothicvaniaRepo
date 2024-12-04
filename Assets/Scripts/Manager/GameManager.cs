using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;

    public CheckPoint[] checkPoints;
    public CheckPoint RebornPoint;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        checkPoints = FindObjectsOfType<CheckPoint>();

        foreach (var point in checkPoints)
        {
            foreach(KeyValuePair<string,bool> pair in _data.checkPoint)
            {
                if (pair.Key==point.Id)
                {
                    point.activtionStatus=pair.Value;
                    if (pair.Value == true)
                    {
                        point.ActivateCheckPoint();

                        if (point.Id == _data.rebornPointId)
                        {
                            point.SetRebornPoint();
                            PlayerManager.instance.player.transform.position=RebornPoint.transform.position;
                        }
                    }
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.checkPoint.Clear();

        _data.rebornPointId = RebornPoint.Id;

        Debug.Log(_data.checkPoint.Count);

        foreach (var point in checkPoints)
        {
            _data.checkPoint.Add(point.Id, point.activtionStatus);
        }
    }
}
