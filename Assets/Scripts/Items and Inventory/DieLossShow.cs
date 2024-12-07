using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieLossShow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.collectLossingCurrency();
            Destroy(gameObject);
        }
    }
}
