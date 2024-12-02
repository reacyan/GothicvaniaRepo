using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string scenesName = "MainScene";
    [SerializeField] private GameObject ContinueButton;
    [SerializeField] UI_FadeScreen fadeScreen;


    private void Start()
    {
        if (!SaveManager.instance.HasSaveData())
        {
            ContinueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScreenWithFadeOut(3));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadScreenWithFadeOut(3));
    }

    public void ExitGame()
    {
        Debug.Log("exit game");
        //Application.Quit();
    }

    IEnumerator LoadScreenWithFadeOut(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay) ;
        SceneManager.LoadScene(scenesName);
    }
}
