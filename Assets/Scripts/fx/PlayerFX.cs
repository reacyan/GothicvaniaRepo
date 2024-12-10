using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{

    [Header("Screen shake Fx")]
    [SerializeField] private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    [SerializeField] private Vector3 shackPower;

    [Header("AfterImageFx")]
    [SerializeField] private GameObject afterImage;
    [SerializeField]private float colorLooseRate;
    [SerializeField] private float colorCooldown;
    private float colorCooldownTimer;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        colorCooldownTimer-=Time.deltaTime;
    }

    public void CreateShadow()
    {
        if (colorCooldownTimer < 0)
        {
            colorCooldownTimer = 0.03f;
            GameObject newShadow = Instantiate(afterImage, transform.position, transform.rotation);
            newShadow.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate);
        }
    }

    public void ScreenShake()
    {
        screenShake.m_DefaultVelocity = new Vector3(shackPower.x * player.facingDir, shackPower.y)*shakeMultiplier;
        screenShake.GenerateImpulse();
    }
}
