using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public string Id;
    public bool activtionStatus;

    [ContextMenu("Generate CheckPoint Id")]
    private void GenerateId()
    {
        Id=System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            SetRebornPoint();
            ActivateCheckPoint();
        }
    }


    public void ActivateCheckPoint()
    {

        Animator anim = GetComponent<Animator>();
        activtionStatus = true;
        anim.SetBool("Active", true);
    }

    public void SetRebornPoint()
    {
        GameManager.instance.RebornPoint = this;
    }
}
