using System;
using UnityEngine;
using UnityEngine.UI;


public class Sword_Skill : Skill
{

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceMaxTravelDistance;

    [Header("peirce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [SerializeField] private float peirceMaxTravelDistance;

    [Header("Spin info")]
    [SerializeField] private float hitCooldown = .3f;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float spinMaxTravelDistance;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float maxTravelDistance;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spacebetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();

        GenereateDots();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);
    }

    private void setupDistance()
    {
        if (baseSkillUnlockButton[1].unlocked)
        {
            maxTravelDistance = spinMaxTravelDistance;

        }
        else if (baseSkillUnlockButton[2].unlocked)
        {
            maxTravelDistance = peirceMaxTravelDistance;
        }
        else if (baseSkillUnlockButton[3].unlocked)
        {
            maxTravelDistance = bounceMaxTravelDistance;
        }
    }

    private void setupGravity()
    {
        if (baseSkillUnlockButton[1].unlocked)
        {
            swordGravity = spinGravity;

        }
        else if (baseSkillUnlockButton[2].unlocked)
        {
            swordGravity = pierceGravity;

        }
        else if (baseSkillUnlockButton[3].unlocked)
        {
            swordGravity = bounceGravity;

        }
    }

    public void SwordCooldown()
    {
        if (player.stats.onSkillBeUse != null)
        {
            player.stats.onSkillBeUse(skillSprite, skillType, cooldown);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spacebetweenDots);
            }
        }

        setupGravity();

        setupDistance();
    }

    public void FindDirction() => finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (baseSkillUnlockButton[1].unlocked)
        {
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown, true);
        }
        else if (baseSkillUnlockButton[2].unlocked)
        {
            newSwordScript.SetupPierce(pierceAmount, maxTravelDistance);
        }
        else if (baseSkillUnlockButton[3].unlocked)
        {
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed, maxTravelDistance);
        }


        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    public bool CanFreezeTimerFor()
    {
        if (branchSkillUnlockButton[0].unlocked)
        {
            return true;
        }

        return false;
    }


    #region Aim region

    //瞄准方向
    private Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }


    //设置瞄准点
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

    #endregion
}
