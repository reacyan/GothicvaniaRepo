using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    private KeyCode myHotKey;

    private TextMeshProUGUI myText;

    private Transform myEnemy;

    private Blackhole_Skill_Controller blackHole;

    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();

        myText = GetComponentInChildren<TextMeshProUGUI>();

        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();//设置热键键位

        myEnemy = _myEnemy.transform;
        blackHole = _myBlackhole;
    }


    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;//设置文本透明
            sr.color = Color.clear;//设置热键透明
        }
    }
}
