using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;

    public int currency;
    public int LossingCurrency;

    public Vector2 DiePosition=Vector2.zero;
    public Player player;
    public GameObject LossingPrefab;

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


    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency = currency - _price;
        return true;
    }

    public int GetCurrentCurrency() => currency;

    public void SetDropCurrency()
    {
        Debug.Log("Drop Currency");
        LossingCurrency = currency;
        currency = 0;
    }

    public void collectLossingCurrency()
    {
        Debug.Log("Pick up");
        currency += LossingCurrency;
    }
    public int GetKonckDir(Transform _targetSource, Transform _attackSource)
    {
        int dir = 0;

        if (_attackSource.position.x > _targetSource.transform.position.x)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        return dir;
    }

    public void LoadData(GameData _data)
    {
        currency = _data.currency;
        LossingCurrency = _data.Lossing;

        if (_data.PlayerDiePosition != Vector2.zero)
        {
            Debug.Log("Drop Currency");
            GameObject DropLossing = Instantiate(LossingPrefab);
            DropLossing.transform.position = _data.PlayerDiePosition;//生成的位置不确定，如果不在地面死亡，掉落物将会掉落在空中，后面将修改这个bug，
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.PlayerDiePosition = Vector2.zero;
        _data.Lossing = this.LossingCurrency;
        _data.currency = this.currency;

        if (DiePosition != Vector2.zero)
        {
            _data.PlayerDiePosition = DiePosition;
        }
    }
}
