using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("AfterImageFx")]
    [SerializeField] private GameObject afterImage;
    [SerializeField]private float colorLooseRate;
    [SerializeField] private float colorCooldown;
    private float colorCooldownTimer;

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
}
