using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_Zone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Entity>() != null)
        {
            collision.GetComponent<Entity>().Die();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
