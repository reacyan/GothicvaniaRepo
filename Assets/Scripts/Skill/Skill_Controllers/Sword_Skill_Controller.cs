using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private float MaxTravelDistance=25;

    private float returnSpeed=12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuartion;

    
    [Header("Pierce info")]
    private float pierceAmount;


    [Header("Bounceing info")]
    private float bounceSpeed;
    private bool isBouncing=false;
    public int BounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float SpinmaxTravelDistance;
    private float spinTimer;
    private float spinDuration;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCooldown;

    private bool isForward;
    private float spinDirection;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

        enemyTarget = new List<Transform>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }


    public void SetupSword(Vector2 _dir,float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        freezeTimeDuartion= _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
        {
            anim.SetBool("Rotation", true);
        }

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        //Invoke("DestroyMe", 7);
    }


    //设置投掷剑的模式参数
    public void SetupBounce(bool _isBouncing, int _amountOfBouunce, float _bounceSpeed, float _maxTravelDistance)
    {
        isBouncing = _isBouncing;
        BounceAmount = _amountOfBouunce;
        bounceSpeed = _bounceSpeed;
        MaxTravelDistance = _maxTravelDistance;
    }

    public void setupPierce(int _pierceAmount,float _maxTravelDistance)
    {
        pierceAmount = _pierceAmount;
        MaxTravelDistance = _maxTravelDistance;
    }

    public void SetupSpin(bool _isSPainning,float _maxTravelDistance,float _spinDuration,float _hitCooldown,bool _isForward)
    {
        isSpinning = _isSPainning;
        SpinmaxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
        isForward = _isForward;
    }


    public void ReturnSword()
    {

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

    }

    private void Update()
    {

        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        if(Vector2.Distance(player.transform.position, transform.position) > MaxTravelDistance)
        {
            Debug.Log("Destroy Me");
            DestroyMe();
        }

        BounceLogic();

        SpinLogic();

    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        if (isForward)
        {
            spinTimer = spinDuration;
            isForward = false;
        }
    }

    void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > SpinmaxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y),1.5f*Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            hit.GetComponent<Enemy>().Damage(player.ishitKonckbback);
                        }
                    }
                }
            }
        }
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {

                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                enemyTarget[targetIndex].GetComponent<Enemy>().Damage(player.ishitKonckbback);
                enemyTarget[targetIndex].GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", freezeTimeDuartion);

                targetIndex++;
                BounceAmount--;

                if (BounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }


        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);
        }

        collision.GetComponent<Enemy>()?.Damage(player.ishitKonckbback);

        SetupTargetsForBounce(collision);

        //Debug.Log(Vector2.Distance(player.transform.position, transform.position));

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {

        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuartion);
        enemy.Damage(player.ishitKonckbback);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {


        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if(isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
