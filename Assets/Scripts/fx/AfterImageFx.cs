using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private float colorLooseRate;

    public void SetupAfterImage(float _looseSpeed)
    {
        colorLooseRate = _looseSpeed;
    }

    private void Update()
    {
        float alpha=sr.color.a - colorLooseRate*Time.deltaTime;

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b,alpha);

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
