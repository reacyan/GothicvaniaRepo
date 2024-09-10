using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;
    private float moveSpeed;
    private bool canMove;
    private bool canExplode;
    private bool canGrow;
    private float growSpeed = 5;


    private Animator anim => GetComponent<Animator>();

    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    public void setCrystal(float _crystalExistDuration, float _moveSpeed, bool _canMove, bool _canExplode)
    {
        crystalExistTimer = _crystalExistDuration;
        moveSpeed = _moveSpeed;
        canMove = _canMove;
        canExplode = _canExplode;
    }

    private void Update()
    {

        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }


    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
