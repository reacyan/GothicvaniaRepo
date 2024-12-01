using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string scenesName = "MainScene";
    [SerializeField] private GameObject ContinueButton;

    private void Start()
    {
        if (!SaveManager.instance.HasSaveData())
        {
            ContinueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(scenesName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(scenesName);
    }

    public void ExitGame()
    {
        Debug.Log("exit game");
        //Application.Quit();
    }
}
