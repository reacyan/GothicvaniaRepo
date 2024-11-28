using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;
    public Player player;
    public int currency;

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
        Debug.Log("money is enough");
        return true;
    }

    public int GetCurrentCurrency() => currency;

    public void LoadData(GameData _data)
    {
        currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
