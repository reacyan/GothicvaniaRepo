using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;

    public void setCrystal(float _crystalExistDuration)
    {
        crystalExistTimer = _crystalExistDuration;
    }

    private void Update()
    {

        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}
